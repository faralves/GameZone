using GameZone.Blog.Application.DTOs;
using GameZone.Blog.Application.DTOs.Request;
using GameZone.Blog.Application.DTOs.Response;
using GameZone.Domain.Contracts.Base;

namespace GameZone.Blog.Application.Interfaces
{
    public interface IComentarioApplication : IGenericCRUD<CreateComentarioDTO, int, UpdateComentarioDTO, ComentarioDTO>
    {
        Task<IEnumerable<ComentarioDTO>> GetByNoticiaId(int id);
    }
}
