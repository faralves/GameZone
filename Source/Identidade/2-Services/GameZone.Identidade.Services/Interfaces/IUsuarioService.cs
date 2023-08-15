using GameZone.Identidade.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GameZone.Identidade.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IdentityResult> CadastrarUsuario(Usuario usuario, string password);
        Task<UsuarioRespostaLogin> LoginUsuario(LoginUsuario usuario);
        Task<Usuario?> ObterUsuarioPorEmail(string email);
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<IList<Claim>> GetRoleClaimsAsync(string roleId);
        Task<IList<String>> GetRolesAsync(Usuario usuario);
        Task<RefreshToken> ObterRefreshToken(Guid refreshToken);
        Task<UsuarioRespostaLogin> GerarJwt(string email);
        Task<Usuario?> GetUser(Guid idUsuario);
    }
}
