using AutoMapper;
using GameZone.Blog.Application.DTOs.Response;
using GameZone.Blog.Application.DTOs.Request;
using GameZone.Blog.Application.Interfaces;
using GameZone.Blog.Domain.Entities;
using GameZone.Blog.Services.Interfaces;

namespace GameZone.Blog.Application
{
    public class ComentarioApplication : IComentarioApplication
    {
        private readonly IComentarioService _comentarioService;
        private IMapper _mapper;

        public ComentarioApplication(IComentarioService comentarioService, IMapper mapper)
        {
            _comentarioService = comentarioService;
            _mapper = mapper;
        }

        public async Task<ComentarioDTO?> Create(CreateComentarioDTO createComentarioDTO, string? idUsuarioClaim)
        {
            createComentarioDTO.AspNetUsersId = new Guid(idUsuarioClaim);

            var comentario = _mapper.Map<Comentarios>(createComentarioDTO);

            await _comentarioService.Create(comentario);

            var comentarioDto = _mapper.Map<ComentarioDTO>(comentario);
            return comentarioDto;
        }

        public async Task Delete(int id)
        {
            var noticia = await _comentarioService.GetById(id);
            await _comentarioService.Delete(id);
        }

        public async Task<IEnumerable<ComentarioDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<ComentarioDTO>>(await _comentarioService.GetAll());
        }

        public async Task<ComentarioDTO?> GetById(int id)
        {
            return _mapper.Map<ComentarioDTO?>(await _comentarioService.GetById(id));
        }

        public async Task<ComentarioDTO?> Update(UpdateComentarioDTO updateComentarioDTO)
        {
            var comentario = _mapper.Map<Comentarios>(updateComentarioDTO);

            await _comentarioService.Update(comentario);

            var comentarioDto = _mapper.Map<ComentarioDTO>(comentario);

            return comentarioDto;
        }

        public async Task<IEnumerable<ComentarioDTO>> GetByNoticiaId(int id)
        {
            return _mapper.Map<IEnumerable<ComentarioDTO>>(await _comentarioService.GetByNoticiaId(id));
        }
    }
}
