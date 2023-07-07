using GameZone.Blog.Domain.Entities;
using GameZone.Domain.Contracts.Base;

namespace GameZone.Blog.Services.Interfaces
{
    public interface INoticiaService : IGenericRepoCRUD<Noticia, int>
    {
    }
}
