using GameZone.News.WebApp.Models.DTO.Request;
using GameZone.News.WebApp.Models.DTO.Response;

namespace GameZone.News.WebApp.Models.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<DTO.Response.CreateNewsDTO>> GetAllNewsAsync();
        Task CreateNewsAsync(DTO.Request.CreateNewsDTO createNewsDto);
        Task<DTO.Request.UpdateNewsDTO?> GetNewsByIdAsync(int id);
        Task UpdateNewsAsync(DTO.Request.UpdateNewsDTO updateNewsDto);
        Task DeleteNewsAsync(int id);
        Task<NewsDto> GetById(int id);
        Task CreateCommentAsync(DTO.Request.CreateCommentDTO newsDto);
    }
}
