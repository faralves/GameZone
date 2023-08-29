using GameZone.News.WebApp.Models;
using GameZone.News.WebApp.Models.Interfaces;
using GameZone.News.WebApp.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameZone.News.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private IAutenticacaoService _autenticacaoService;
        private readonly IConfiguration _configuration;
        private static bool _local_execution = false;


        public HomeController(ILogger<HomeController> logger, INewsService newsService, IConfiguration configuration, IAutenticacaoService autenticacaoService)
        {
            _logger = logger;
            _newsService = newsService;
            _configuration = configuration;
            _local_execution = bool.Parse(_configuration.GetSection("EnableLocalExecution").Value);
            _autenticacaoService = autenticacaoService;
        }

        public async Task<IActionResult> Index()
        {
            //try
            //{
                // Simulando um erro de programação
                //throw new System.Exception("Esta é uma simulação de erro.");

                var news = await _newsService.GetAllNewsAsync();
                if (news.Any())
                {
                    foreach (var noticia in news)
                    {
                        var usuarioAutor = await _autenticacaoService.GetUserDto(noticia.AspNetUsersId);
                        noticia.Autor = usuarioAutor.Name;
                    }
                }

                ViewBag.LocalExecution = _local_execution;

                return View(news);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("sistema-indisponivel")]
        public IActionResult SistemaIndisponivel()
        {
            var modelErro = new ErrorViewModel
            {
                Mensagem = "O sistema está temporariamente indisponível, isto pode ocorrer em momentos de sobrecarga de usuários.",
                Titulo = "Sistema indisponível.",
                ErroCode = 500
            };

            return View("Error", modelErro);
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelErro = new ErrorViewModel();

            var exceptionDetails = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

            ViewBag.ErrorMessage = exceptionDetails?.Error?.Message;
            ViewBag.ErrorPath = exceptionDetails?.Path;
            ViewBag.ErrorStackTrace = exceptionDetails?.Error?.StackTrace;

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}