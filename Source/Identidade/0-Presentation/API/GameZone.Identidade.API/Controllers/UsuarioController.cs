using GameZone.Identidade.API.Authorization;
using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        //[Authorize]
        //[InserirUsuarioAuthorization]
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

                var usuarioRespostaLogin = await _usuarioApplication.LoginUsuario(loginUsuarioDto);

                if (usuarioRespostaLogin != null)
                {
                    _logger.LogInformation("Usuário: " + loginUsuarioDto.UserName + " autenticado com sucesso!"); 

                    return Ok(usuarioRespostaLogin);
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
                _logger.LogInformation("Ocorreu erro para fazer o login: "  + Environment.NewLine + " Erro: " + ex.Message);

                var erro = new { erro =  ex.Message, loginUsuarioDto };

                return BadRequest(erro);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh Token inválido");
            }

            var token = await _usuarioApplication.ObterRefreshToken(Guid.Parse(refreshToken));

            if (token is null)
            {
                return BadRequest("Refresh Token expirado");
            }

            return Ok(await _usuarioApplication.GerarJwt(token.Username));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("O formato do Id do Usuário não está correto. " + Environment.NewLine + "ModelState: " + ModelState);

                return BadRequest(ModelState);
            }

            var usuarioRespostaLogin = await _usuarioApplication.GetUser(id);

            if (usuarioRespostaLogin != null)
                return Ok(usuarioRespostaLogin);
            else
                return NoContent();
        }
    }
}
