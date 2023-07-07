using GameZone.Blog.Domain.Entities;
using GameZone.Blog.Infra.Data;
using Microsoft.EntityFrameworkCore;
using GameZone.Blog.Infra.Interfaces;

namespace GameZone.Blog.Infra.Repository
{
    public class NoticiaRepository : INoticiaRepository
    {
        private readonly ApplicationDbContext _context;

        public NoticiaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Noticia noticia)
        {
            await _context.Noticias.AddAsync(noticia);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.Noticias.Remove(await _context.Noticias.FirstOrDefaultAsync(n => n.Id == id));
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Noticia>> GetAll()
        {
            return await _context.Noticias.ToListAsync();
        }

        public async Task<Noticia?> GetById(int id)
        {
            var noticia = await _context.Noticias.FirstOrDefaultAsync(n => n.Id == id);
            if (noticia != null)
                _context.Entry(noticia).State = EntityState.Detached;

            return noticia;
        }

        public async Task Update(Noticia noticia)
        {
            _context.Noticias.Update(noticia);
            await _context.SaveChangesAsync();
        }
    }
}
