using GameZone.Blog.Domain.Entities;
using GameZone.Blog.Infra.Interfaces;
using GameZone.Blog.Services.Interfaces;
using Newtonsoft.Json;

namespace GameZone.Blog.Services
{
    public class ComentarioService : IComentarioService
    {
        private readonly IComentarioRepository _comentarioRepository;

        public ComentarioService(IComentarioRepository comentarioRepository)
        {
            _comentarioRepository = comentarioRepository;
        }

        public async Task Create(Comentarios comentario)
        {            
            await _comentarioRepository.Create(comentario); 
        }

        public Task Delete(int id)
        {
            return _comentarioRepository.Delete(id);
        }

        public async Task<IEnumerable<Comentarios>> GetAll()
        {
            return await _comentarioRepository.GetAll();
        }

        public async Task<Comentarios> GetById(int id)
        {
            return await _comentarioRepository.GetById(id);
        }

        public async Task Update(Comentarios comentario)
        {
            await _comentarioRepository.Update(comentario);
        }             
        
        public async Task<IEnumerable<Comentarios>> GetByNoticiaId(int id)
        {
            var listaComentarios = await _comentarioRepository.GetByNoticiaId(id);

            return listaComentarios;
        }
    }
}
