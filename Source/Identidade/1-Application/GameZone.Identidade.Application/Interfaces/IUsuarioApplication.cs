using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Domain.Entidades;
using Microsoft.AspNetCore.Identity;

namespace GameZone.Identidade.Application.Interfaces
{
    public interface IUsuarioApplication
    {
        Task<IdentityResult> CadastrarUsuario(CreateUsuarioDto usuarioDto);
        Task<string> LoginUsuario(LoginUsuarioDto loginUsuarioDto);
        Task<Usuario?> ObterUsuarioPorEmail(string email);
    }
}
