using GameZone.Identidade.Domain.Entidades;
using GameZone.Identidade.Infra.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GameZone.Identidade.Infra.Repository
{
    public class IdentidadeRepository : IIdentidadeRepository
    {
        private UserManager<Usuario> _userManager;
        private SignInManager<Usuario> _signInManager;
        private readonly ILogger<IdentidadeRepository> _logger;

        public IdentidadeRepository(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ILogger<IdentidadeRepository> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IdentityResult> CadastrarUsuario(Usuario usuario, string password)
        {
            try
            {
                var resultado = await _userManager.CreateAsync(usuario, password);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ocorreu erro para cadastrar o usuário.");
                throw;
            }
        }

        public async Task<Usuario?> LoginUsuario(Usuario usuario)
        {
            //var resultado = await _signInManager.PasswordSignInAsync(loginUsuarioDto.Email, loginUsuarioDto.Password, false, false);

            Usuario? usuarioDb = _signInManager.UserManager.Users.FirstOrDefault(user => user.NormalizedUserName == usuario.UserName.ToUpper());

            return usuarioDb;
        }

        public async Task<Usuario?> ObterUsuarioPorEmail(string email)
        {
            Usuario? usuarioDb = await _userManager.FindByEmailAsync(email);

            return usuarioDb;
        }
    }
}
