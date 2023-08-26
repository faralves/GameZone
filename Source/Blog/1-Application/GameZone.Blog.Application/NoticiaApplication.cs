using AutoMapper;
using GameZone.Blog.Application.DTOs.Response;
using GameZone.Blog.Application.DTOs.Request;
using GameZone.Blog.Application.Interfaces;
using GameZone.Blog.Domain.Entities;
using GameZone.Blog.Services.Interfaces;
using GameZone.Domain.Contracts.Base;
using System;
using Microsoft.Extensions.Configuration;

namespace GameZone.Blog.Application
{
    public class NoticiaApplication : INoticiaApplication
    {
        private readonly INoticiaService _noticiaService;
        private IMapper _mapper;
        private static bool _local_execution = false;
        private readonly IConfiguration _configuration;
        private static string containerBlobStorage = string.Empty;


        public NoticiaApplication(INoticiaService noticiaService, IMapper mapper, IConfiguration configuration)
        {
            _noticiaService = noticiaService;
            _mapper = mapper;
            _configuration = configuration;
            _local_execution = Boolean.Parse(_configuration.GetSection("EnableLocalExecution").Value);
            containerBlobStorage = _configuration.GetSection("ConfigsAzure:ContainerBlobStorage").Value;
        }

        public async Task<NoticiaDTO?> Create(CreateNoticiaDTO createNoticiaDTO, string? idUsuarioClaim)
        {
            if (!_local_execution && !string.IsNullOrEmpty(createNoticiaDTO.UrlImagem))
            {
                createNoticiaDTO.UrlBlobStorage = _noticiaService.UploadBase64ImageBlobStorage(createNoticiaDTO.Database64Content, containerBlobStorage, createNoticiaDTO.UrlImagem);
                createNoticiaDTO.UrlImagem = createNoticiaDTO.UrlBlobStorage;
            }

            var noticia = _mapper.Map<Noticia>(createNoticiaDTO);

            noticia.AspNetUsersId = idUsuarioClaim;

            await _noticiaService.Create(noticia);

            var noticiaDto = _mapper.Map<NoticiaDTO>(noticia);
            return noticiaDto;
        }

        public async Task Delete(int id)
        {
            var noticia = await _noticiaService.GetById(id);
            bool deletedImageBlobStorage = false;
            if (noticia != null)
            {
                if(!string.IsNullOrEmpty(noticia.UrlImagem))
                {
                    var uri = new Uri(noticia.UrlImagem);
                    deletedImageBlobStorage = _noticiaService.DeleteImageBlobStorage(uri, containerBlobStorage);
                }
            }

            await _noticiaService.Delete(id);
        }

        public async Task<IEnumerable<NoticiaDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<NoticiaDTO>>(await _noticiaService.GetAll());
        }

        public async Task<NoticiaDTO?> GetById(int id)
        {
            return _mapper.Map<NoticiaDTO?>(await _noticiaService.GetById(id));
        }

        public async Task<NoticiaDTO?> Update(UpdateNoticiaDTO updateNoticiaDTO, string idUsuario)
        {
            var noticiaDb = await GetById(updateNoticiaDTO.Id);

            if (!_local_execution && !string.IsNullOrEmpty(updateNoticiaDTO.UrlImagem) && updateNoticiaDTO.UrlImagem != noticiaDb.UrlImagem)
            {
                updateNoticiaDTO.UrlBlobStorage = _noticiaService.UploadBase64ImageBlobStorage(updateNoticiaDTO.Database64Content, containerBlobStorage, updateNoticiaDTO.UrlImagem);
                updateNoticiaDTO.UrlImagem = updateNoticiaDTO.UrlBlobStorage;
            }

            var noticia = _mapper.Map<Noticia>(updateNoticiaDTO);
            noticia.AspNetUsersId = idUsuario;

            await _noticiaService.Update(noticia);

            var noticiaDto = _mapper.Map<NoticiaDTO>(noticia);

            return noticiaDto;
        }
    }
}
