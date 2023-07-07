using GameZone.Blog.Domain.Entities;
using GameZone.Blog.Infra.Interfaces;
using GameZone.Blog.Services.Interfaces;

namespace GameZone.Blog.Services
{
    public class NoticiaService : INoticiaService
    {
        private readonly INoticiaRepository _noticiaRepository;

        public NoticiaService(INoticiaRepository noticiaRepository)
        {
            _noticiaRepository = noticiaRepository;
        }

        public async Task Create(Noticia noticia)
        {            
            await _noticiaRepository.Create(noticia); 
        }

        public Task Delete(int id)
        {
            return _noticiaRepository.Delete(id);
        }

        public async Task<IEnumerable<Noticia>> GetAll()
        {
            return await _noticiaRepository.GetAll();
        }

        public async Task<Noticia> GetById(int id)
        {
            return await _noticiaRepository.GetById(id);
        }

        public async Task Update(Noticia noticia)
        {
            await _noticiaRepository.Update(noticia);
        }

    }
}
