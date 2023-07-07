using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Identidade.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioApplication _usuarioApplication;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioApplication usuarioApplication, ILogger<UsuarioController> logger)
        {
            _usuarioApplication = usuarioApplication;
            _logger = logger;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario(CreateUsuarioDto usuarioDto) 
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("A requisição não teve sucesso. " + Environment.NewLine + "ModelState: " + ModelState);

                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Cadastrando o usuário " + Environment.NewLine + "Usuário: " + usuarioDto.Name);

                var resultado = await _usuarioApplication.CadastrarUsuario(usuarioDto);

                if (resultado.Succeeded)
                {
                    _logger.LogInformation("Usuário: " + usuarioDto.UserName + " Cadastrado com sucesso!");

                    return Ok(usuarioDto);
                }
                else
                {
                    _logger.LogInformation("Usuário: " + usuarioDto.UserName + " não cadastrado!");

                    return BadRequest(resultado);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Ocorreu erro para cadastrar o usuário: " + usuarioDto.UserName + " não cadastrado!" + Environment.NewLine + " Erro: "+ ex.Message);
                return BadRequest(usuarioDto);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUsuarioDto loginUsuarioDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("A requisição não teve sucesso. " + Environment.NewLine + "ModelState: " + ModelState);

                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Autenticando o usuário " + Environment.NewLine + "Email: " + loginUsuarioDto.Email);

                var token = await _usuarioApplication.LoginUsuario(loginUsuarioDto);

                if (!string.IsNullOrEmpty(token))
                {
                    _logger.LogInformation("Usuário: " + loginUsuarioDto.UserName + " autenticado com sucesso!" + Environment.NewLine + " Token: " + token); 

                    return Ok(new { Token = token });
                }
                else
                {
                    _logger.LogInformation("Usuário: " + loginUsuarioDto.UserName + " usuário ou senha incorretos.");

                    return Unauthorized(new { Mensagem = "Credenciais inválidas" }); 
                }
            }
            catch(ArgumentNullException ArgEx)
            {
                _logger.LogInformation("Usuário: " + loginUsuarioDto.UserName + " usuário ou senha incorretos.");

                return Unauthorized(new { Mensagem = "Credenciais inválidas" }); ;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Ocorreu erro para cadastrar o usuário: " + loginUsuarioDto.UserName + " não cadastrado!" + Environment.NewLine + " Erro: " + ex.Message);
                return BadRequest(loginUsuarioDto);
            }
        }

    }
}
