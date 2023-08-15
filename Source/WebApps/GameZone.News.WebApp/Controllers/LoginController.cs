using GameZone.Core.DomainObjects;
using GameZone.News.WebApp.Models.DTO.Request;
using GameZone.News.WebApp.Models.Interfaces;
using GameZone.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

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
            ViewData["ReturnUrl"] = returnUrl;
            if (_appUser.EstaAutenticado())
            {
                if (string.IsNullOrEmpty(returnUrl)) 
                    return RedirectToAction("Index", "Home");

                return LocalRedirect(returnUrl);
            }

            return View();
        }

        [HttpPost]
        [Route("Logar")]
        public async Task<IActionResult> Logar(LoginDTO loginDTO, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(loginDTO);

            var resposta = await _autenticacaoService.Logar(loginDTO);

            if (resposta != null)
                await _autenticacaoService.RealizarLogin(resposta);

            if (_appUser.EstaAutenticado())
            {
                if (string.IsNullOrEmpty(returnUrl)) 
                    return RedirectToAction("Index", "Home");

                return LocalRedirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Credenciais inválidas");
                return View();
            }
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _autenticacaoService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("CadastrarUsuario")]
        public IActionResult CadastrarUsuario(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }


        [HttpPost]
        [Route("CadastrarUsuario")]
        public async Task<IActionResult> CadastrarUsuario(CreateUserDTO createUserDto, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(createUserDto);

            var resposta = await _autenticacaoService.CadastrarUsuario(createUserDto);

            if (resposta != null)
                await _autenticacaoService.RealizarLogin(resposta);

            if (_appUser.EstaAutenticado())
            {
                if (string.IsNullOrEmpty(returnUrl)) 
                    return RedirectToAction("Index", "Home");

                return LocalRedirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Credenciais inválidas");
                return View();
            }
        }
    }
}
