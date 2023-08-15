using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Application.DTOs.Response;
using GameZone.Identidade.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GameZone.Identidade.Application.Interfaces
{
    public interface IUsuarioApplication
    {
        Task<IdentityResult> CadastrarUsuario(CreateUsuarioDto usuarioDto);
        Task<UsuarioRespostaLogin> LoginUsuario(LoginUsuarioDto loginUsuarioDto);
        Task<Usuario?> ObterUsuarioPorEmail(string email);
        Task<IList<String>> GetRolesAsync(Usuario usuario);
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<IList<Claim>> GetRoleClaimsAsync(string roleId);
        Task<RefreshToken> ObterRefreshToken(Guid refreshToken);
        Task<UsuarioRespostaLogin> GerarJwt(string email);
        Task<UsuarioDto> GetUser(Guid idUsuario);
    }
}
