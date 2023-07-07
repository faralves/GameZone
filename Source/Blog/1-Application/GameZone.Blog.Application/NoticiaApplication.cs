using AutoMapper;
using GameZone.Blog.Application.DTOs;
using GameZone.Blog.Application.Interfaces;
using GameZone.Blog.Domain.Entities;
using GameZone.Blog.Services.Interfaces;

namespace GameZone.Blog.Application
{
    public class NoticiaApplication : INoticiaApplication
    {
        private readonly INoticiaService _noticiaService;
        private IMapper _mapper;

        public NoticiaApplication(INoticiaService noticiaService, IMapper mapper)
        {
            _noticiaService = noticiaService;
            _mapper = mapper;
        }

        public async Task<NoticiaDTO?> Create(CreateNoticiaDTO createNoticiaDTO)
        {
            var noticia = _mapper.Map<Noticia>(createNoticiaDTO);

            await _noticiaService.Create(noticia);

            var noticiaDto = _mapper.Map<NoticiaDTO>(noticia);
            return noticiaDto;
        }

        public async Task Delete(int id)
        {
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

        public async Task<NoticiaDTO?> Update(NoticiaDTO updateNoticiaDTO)
        {
            var noticia = _mapper.Map<Noticia>(updateNoticiaDTO);

            await _noticiaService.Update(noticia);

            var noticiaDto = _mapper.Map<NoticiaDTO>(noticia);

            return noticiaDto;
        }
    }
}
