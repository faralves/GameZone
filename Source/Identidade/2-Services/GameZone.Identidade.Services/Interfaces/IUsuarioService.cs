using GameZone.Identidade.Domain.Entidades;
using Microsoft.AspNetCore.Identity;

namespace GameZone.Identidade.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IdentityResult> CadastrarUsuario(Usuario usuario, string password);
        Task<string> LoginUsuario(Usuario usuario);
        Task<Usuario?> ObterUsuarioPorEmail(string email);
    }
}
