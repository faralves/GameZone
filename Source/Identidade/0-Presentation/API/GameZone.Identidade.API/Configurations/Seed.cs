using GameZone.Identidade.API.Configurations.Interfaces;
using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Application.Interfaces;
using GameZone.Identidade.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GameZone.Identidade.API.Configurations
{
    public class Seed : ISeed
    {
        private IUsuarioApplication _usuarioApplication;
        private readonly ILogger<Seed> _logger;
        private UserManager<Usuario> _userManager;
        public Seed(IUsuarioApplication usuarioApplication, ILogger<Seed> logger, UserManager<Usuario> userManager)
        {
            _usuarioApplication = usuarioApplication;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task UsuarioAdm()
        {
            var email = "admin@gamezone.com.br";
            
            var adminUser = await _usuarioApplication.ObterUsuarioPorEmail(email);
            if (adminUser == null)
            {
                CreateUsuarioDto usuarioDto = new CreateUsuarioDto("Admin User", "12345678901", DateTime.Now.Date, "admin@gamezone.com.br", "123@Mudar", "123@Mudar", true, "" );
                var resultadoCadastro = _usuarioApplication.CadastrarUsuario(usuarioDto).Result;
                if(resultadoCadastro.Succeeded)
                    _logger.LogInformation("Usuário: " + usuarioDto.UserName + " cadastrado com sucesso!");
                else
                    _logger.LogError("Usuário: " + usuarioDto.UserName + " erro para cadastro. " + Environment.NewLine + " Erro: " + resultadoCadastro.Errors);
            }
        }
    }
}
