using GameZone.Core.DomainObjects;
using GameZone.Identidade.Domain.Entities;
using GameZone.Identidade.Infra.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace GameZone.Identidade.Infra.Repository
{
    public class IdentidadeRepository : IIdentidadeRepository
    {
        private UserManager<Usuario> _userManager;
        private SignInManager<Usuario> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ILogger<IdentidadeRepository> _logger;
        private readonly AuthenticationRepository _authenticationRepository;

        public IdentidadeRepository(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ILogger<IdentidadeRepository> logger, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CadastrarUsuario(Usuario usuario, string password)
        {
            try
            {
                var resultado = await _userManager.CreateAsync(usuario, password);

                if (resultado.Succeeded)
                {
                    // Adiciona claims personalizadas
                    //await _userManager.AddClaimAsync(usuario, new Claim("ClaimAdministrator", usuario.IsAdministrator.ToString()));

                    // Adiciona roles ao usuário
                    if (usuario.IsAdministrator)
                        await _userManager.AddToRoleAsync(usuario, "Administrador");
                    else
                        await _userManager.AddToRoleAsync(usuario, "Usuário");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ocorreu erro para cadastrar o usuário.");
                throw;
            }
        }

        public async Task<SignInResult> LoginUsuario(LoginUsuario usuario)
        {
            return await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Password, false, true); ;
        }
        
        public async Task<IList<Claim>> GetClaimsAsync(Usuario usuario)
        {
            var userClaims = await _signInManager.UserManager.GetClaimsAsync(usuario);

            return userClaims;
        }
        public async Task<IList<String>> GetRolesAsync(Usuario usuario)
        {
            if (usuario != null)
            {
                var roles = await _signInManager.UserManager.GetRolesAsync(_signInManager.UserManager.Users.FirstOrDefault(user => user.NormalizedUserName == usuario.UserName.ToUpper()));
                return roles.ToList();
            }

            return new List<string>();
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return role;
        }
        public async Task<IList<Claim>> GetRoleClaimsAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                return roleClaims;
            }
            return new List<Claim>();
        }


        public async Task<Usuario?> ObterUsuarioPorEmail(string email)
        {
            Usuario? usuarioDb = await _userManager.FindByEmailAsync(email);

            return usuarioDb;
        }

        public async Task<Usuario?> GetUser(Guid idUsuario)
        {
            Usuario? usuarioDb = await _userManager.FindByIdAsync(idUsuario.ToString());

            return usuarioDb;
        }
    }
}
