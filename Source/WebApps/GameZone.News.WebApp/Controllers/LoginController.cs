using GameZone.News.WebApp.Models.DTO.Request;
using GameZone.News.WebApp.Models.Interfaces;
using GameZone.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.News.WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly IAspNetUser _appUser;

        public LoginController(IAutenticacaoService autenticacaoService, IAspNetUser appUser)
        {
            _autenticacaoService = autenticacaoService;
            _appUser = appUser;
        }

        [HttpGet]
        [Route("Logar")]
        public IActionResult Logar(string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                if (_appUser.EstaAutenticado())
                {
                    if (string.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Home");

                    return LocalRedirect(returnUrl);
                }

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("Logar")]
        public async Task<IActionResult> Logar(LoginDTO loginDTO, string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                if (!ModelState.IsValid)
                    return View(loginDTO);

                var resposta = await _autenticacaoService.Logar(loginDTO);

                if (resposta != null)
                    await _autenticacaoService.RealizarLogin(resposta);
                else
                {
                    ModelState.AddModelError("Login", "As Credenciais não conferem ou não existem.");
                    return View(loginDTO);
                }

                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("Index", "Home");

                return LocalRedirect(returnUrl);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _autenticacaoService.Logout();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("CadastrarUsuario")]
        public IActionResult CadastrarUsuario(string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("CadastrarUsuario")]
        public async Task<IActionResult> CadastrarUsuario(CreateUserDTO createUserDto, string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                if (!ModelState.IsValid)
                    return View(createUserDto);

                if (User.Identity.IsAuthenticated)
                    createUserDto.IdUsuarioInclusao = User.GetUserId();

                var resposta = await _autenticacaoService.CadastrarUsuario(createUserDto);

                if (resposta != null)
                    await _autenticacaoService.RealizarLogin(resposta);

                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("Index", "Home");
                return LocalRedirect(returnUrl);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
