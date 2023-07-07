using GameZone.Identidade.API.Configurations.Interfaces;
using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Application.Interfaces;
using GameZone.Identidade.Domain.Entidades;
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
                CreateUsuarioDto usuarioDto = new CreateUsuarioDto() { Name = "Admin User", CpfCnpj = "12345678901", DataNascimento = DateTime.Now.Date, Email = "admin@gamezone.com.br", IdUsuarioInclusao= "", IsAdministrator = true, Password = "123@Mudar", RePassword = "123@Mudar"};
                var resultadoCadastro = _usuarioApplication.CadastrarUsuario(usuarioDto).Result;
                if(resultadoCadastro.Succeeded)
                    _logger.LogInformation("Usuário: " + usuarioDto.UserName + " cadastrado com sucesso!");
                else
                    _logger.LogError("Usuário: " + usuarioDto.UserName + " erro para cadastro. " + Environment.NewLine + " Erro: " + resultadoCadastro.Errors);
            }
        }
    }
}
