using GameZone.News.WebApp.Models.DTO.Request;
using GameZone.News.WebApp.Models.Interfaces;
using GameZone.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace GameZone.News.WebApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IConfiguration _configuration;
        private static bool _local_execution = false;

        public NewsController(INewsService newsService, IConfiguration configuration)
        {
            _newsService = newsService;
            _configuration = configuration;
            _local_execution = bool.Parse(_configuration.GetSection("EnableLocalExecution").Value);
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var news = await _newsService.GetAllNewsAsync();
            var pagedNews = news.ToPagedList(pageNumber, pageSize);

            return View(pagedNews);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var noticia = await _newsService.GetById(id);
            if (noticia == null)
            {
                return NotFound();
            }
            noticia.CreateComentario = new Models.DTO.Response.CreateCommentDTO() {Comentario = string.Empty};
            ViewBag.LocalExecution = _local_execution;

            return View(noticia);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsDTO newsDto)
        {
            if (!ModelState.IsValid)
            {
                return View(newsDto);
            }

            await _newsService.CreateNewsAsync(newsDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Models.DTO.Request.UpdateNewsDTO? updateNewsDto)
        {
            if (updateNewsDto == null || id != updateNewsDto.Id)
            {
                return BadRequest();
            }

            var updateNews = await _newsService.GetNewsByIdAsync(updateNewsDto.Id);

            if (!ModelState.IsValid)
            {
                return View(updateNewsDto);
            }

            if (User.Identity.IsAuthenticated)
                updateNewsDto.UsuarioId = new Guid(User.GetUserId());

            await _newsService.UpdateNewsAsync(updateNewsDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _newsService.DeleteNewsAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment(CreateCommentDTO createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                if (createCommentDto?.NoticiaId != 0)
                    return RedirectToAction("GetById", new { id = createCommentDto.NoticiaId });
                else
                    return RedirectToAction("Index", "Home");
            }

            await _newsService.CreateCommentAsync(createCommentDto);
            return RedirectToAction("GetById", new { id = createCommentDto.NoticiaId });
        }
    }
}