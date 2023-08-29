using GameZone.News.WebApp.Models.DTO.Request;
using GameZone.News.WebApp.Models.Interfaces;
using GameZone.News.WebApp.Models.Services;
using GameZone.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace GameZone.News.WebApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private IAutenticacaoService _autenticacaoService;
        private readonly IConfiguration _configuration;
        private static bool _local_execution = false;

        public NewsController(INewsService newsService, IConfiguration configuration, IAutenticacaoService autenticacaoService)
        {
            _newsService = newsService;
            _configuration = configuration;
            _local_execution = bool.Parse(_configuration.GetSection("EnableLocalExecution").Value);
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                int pageSize = 10;
                int pageNumber = page ?? 1;

                var news = await _newsService.GetAllNewsAsync();
                if (news.Any())
                {
                    foreach (var noticia in news)
                    {
                        var usuarioAutor = await _autenticacaoService.GetUserDto(noticia.AspNetUsersId);
                        noticia.Autor = usuarioAutor.Name;
                    }
                }

                var pagedNews = news.ToPagedList(pageNumber, pageSize);

                return View(pagedNews);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var noticia = await _newsService.GetById(id);
                if (noticia == null)
                {
                    return NotFound();
                }
                noticia.CreateComentario = new Models.DTO.Response.CreateCommentDTO() { Comentario = string.Empty };
                ViewBag.LocalExecution = _local_execution;

                return View(noticia);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsDTO newsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(newsDto);
                }

                await _newsService.CreateNewsAsync(newsDto);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var news = await _newsService.GetNewsByIdAsync(id);
                if (news == null)
                {
                    return NotFound();
                }

                return View(news);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Models.DTO.Request.UpdateNewsDTO? updateNewsDto)
        {
            try
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

                var usuarioAutor = await _autenticacaoService.GetUserDto(updateNewsDto.UsuarioId);
                updateNewsDto.Autor = usuarioAutor.Name;

                await _newsService.UpdateNewsAsync(updateNewsDto);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _newsService.DeleteNewsAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment(CreateCommentDTO createCommentDto)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}