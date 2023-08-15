using GameZone.Blog.Application.DTOs;
using GameZone.Blog.Application.DTOs.Request;
using GameZone.Blog.Application.DTOs.Response;
using GameZone.Domain.Contracts.Base;

namespace GameZone.Blog.Application.Interfaces
{
    public interface INoticiaApplication : IGenericCRUD<CreateNoticiaDTO, int, UpdateNoticiaDTO, NoticiaDTO>
    {
    }
}
