using GameZone.Identidade.Domain.Entities;
using GameZone.Identidade.Infra.Interfaces;
using GameZone.Identidade.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace GameZone.Identidade.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ILogger<UsuarioService> _logger;
        private IIdentidadeRepository _identidadeRepository;
        private IAuthenticationRepository _authenticationRepository;



        public UsuarioService(ILogger<UsuarioService> logger,  IIdentidadeRepository identidadeRepository, IAuthenticationRepository authenticationRepository)
        {
            _logger = logger;
            _identidadeRepository = identidadeRepository;
            _authenticationRepository = authenticationRepository;
        }

        public async Task<IdentityResult> CadastrarUsuario(Usuario usuario, string password)
        {
            try
            {
                return await _identidadeRepository.CadastrarUsuario(usuario, password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ocorreu erro para cadastrar o usuário.");
                throw;
            }
        }

        public async Task<UsuarioRespostaLogin> LoginUsuario(LoginUsuario usuario)
        {
            var result = await _identidadeRepository.LoginUsuario(usuario);

            if (result.IsLockedOut)
                throw new Exception("Conta Bloqueada");

            if (!result.Succeeded)
                throw new Exception("Usuário ou Senha Incorretos");

            if (result.Succeeded)
                return await _authenticationRepository.GerarJwt(usuario.Email);
            
            UsuarioRespostaLogin usuarioRespostaLogin = null;
            return usuarioRespostaLogin;
        }
        public async Task<Usuario?> ObterUsuarioPorEmail(string email)
        {
            var usuarioDb = await _identidadeRepository.ObterUsuarioPorEmail(email);
            return usuarioDb;
        }

        public async Task<IList<string>> GetRolesAsync(Usuario usuario)
        {
            var roles = await _identidadeRepository.GetRolesAsync(usuario);

            return roles;
        }
        public async Task<IdentityRole> GetRoleByIdAsync(string roleName)
        {
            var role = await _identidadeRepository.GetRoleByIdAsync(roleName);

            return role;
        }

        public async Task<IList<Claim>> GetRoleClaimsAsync(string roleName)
        {
            var roleClaims = await _identidadeRepository.GetRoleClaimsAsync(roleName);

            return roleClaims;
        }

        public async Task<RefreshToken> ObterRefreshToken(Guid refreshToken)
        {
            return await _authenticationRepository.ObterRefreshToken(refreshToken);
        }

        public async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            return await _authenticationRepository.GerarJwt(email);
        }

        public async Task<Usuario?> GetUser(Guid idUsuario)
        {
            var usuarioDb = await _identidadeRepository.GetUser(idUsuario);
            return usuarioDb;
        }

    }
}
