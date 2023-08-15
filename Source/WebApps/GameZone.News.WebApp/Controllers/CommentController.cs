using GameZone.News.WebApp.Models;
using GameZone.News.WebApp.Models.DTO.Request;
using GameZone.News.WebApp.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameZone.News.WebApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly INewsService _newsService;

        public CommentController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.DTO.Response.NewsDto newsDto)
        {
            if (!ModelState.IsValid)
            {
                return View(newsDto);
            }

            CreateCommentDTO createCommentDto = new CreateCommentDTO();
            await _newsService.CreateCommentAsync(createCommentDto);
            return RedirectToAction("Index", "News");
        }
    }
}
