using GameZone.Blog.Domain.Entities;
using GameZone.Blog.Infra.Data;
using Microsoft.EntityFrameworkCore;
using GameZone.Blog.Infra.Interfaces;

namespace GameZone.Blog.Infra.Repository
{
    public class ComentarioRepository : IComentarioRepository
    {
        private readonly ApplicationDbContext _context;

        public ComentarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Comentarios comentario)
        {
            await _context.Comentarios.AddAsync(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.Comentarios.Remove(await _context.Comentarios.FirstOrDefaultAsync(n => n.Id == id));
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comentarios>> GetAll()
        {
            return await _context.Comentarios.ToListAsync();
        }

        public async Task<Comentarios?> GetById(int id)
        {
            var comentario = await _context.Comentarios.FirstOrDefaultAsync(n => n.Id == id);
            if (comentario != null)
                _context.Entry(comentario).State = EntityState.Detached;

            return comentario;
        }

        public async Task Update(Comentarios comentario)
        {
            _context.Comentarios.Update(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comentarios>> GetByNoticiaId(int id)
        {
            var comentario = await _context.Comentarios.Where(n => n.NoticiaId == id).ToListAsync();
            
            return comentario;
        }
    }
}
