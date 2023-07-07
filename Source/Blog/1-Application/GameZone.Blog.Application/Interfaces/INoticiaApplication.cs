using GameZone.Blog.Application.DTOs;
using GameZone.Domain.Contracts.Base;

namespace GameZone.Blog.Application.Interfaces
{
    public interface INoticiaApplication : IGenericCRUD<CreateNoticiaDTO, int, NoticiaDTO>
    {
    }
}
