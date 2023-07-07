using GameZone.Identidade.Domain.Entidades;
using Microsoft.AspNetCore.Identity;

namespace GameZone.Identidade.Infra.Interfaces
{
    public interface IIdentidadeRepository
    {
        Task<IdentityResult> CadastrarUsuario(Usuario usuario, string password);
        Task<Usuario?> LoginUsuario(Usuario usuario);
        Task<Usuario?> ObterUsuarioPorEmail(string email);
    }
}
