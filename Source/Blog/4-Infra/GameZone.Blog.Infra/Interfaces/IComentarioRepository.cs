using GameZone.Blog.Domain.Entities;
using GameZone.Domain.Contracts.Base;

namespace GameZone.Blog.Infra.Interfaces
{
    public interface IComentarioRepository : IGenericRepoCRUD<Comentarios, int>
    {
        Task<IEnumerable<Comentarios>> GetByNoticiaId(int id);
    }
}
