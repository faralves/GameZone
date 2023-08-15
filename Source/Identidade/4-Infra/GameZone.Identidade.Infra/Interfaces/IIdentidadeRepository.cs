using GameZone.Identidade.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GameZone.Identidade.Infra.Interfaces
{
    public interface IIdentidadeRepository
    {
        Task<IdentityResult> CadastrarUsuario(Usuario usuario, string password);
        Task<SignInResult> LoginUsuario(LoginUsuario usuario);
        Task<IList<Claim>> GetClaimsAsync(Usuario usuario);
        Task<IList<String>> GetRolesAsync(Usuario usuario);
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<IList<Claim>> GetRoleClaimsAsync(string roleId);

        Task<Usuario?> ObterUsuarioPorEmail(string email);
        Task<Usuario?> GetUser(Guid idUsuario);
    }
}
