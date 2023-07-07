using GameZone.Blog.Domain.Entities;
using GameZone.Domain.Contracts.Base;

namespace GameZone.Blog.Infra.Interfaces
{
    public interface INoticiaRepository : IGenericRepoCRUD<Noticia, int>
    {
    }
}
