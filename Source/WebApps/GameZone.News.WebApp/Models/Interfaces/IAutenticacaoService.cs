using GameZone.News.WebApp.Models.DTO.Request;
using GameZone.News.WebApp.Models.DTO.Response;
using System.IdentityModel.Tokens.Jwt;

namespace GameZone.News.WebApp.Models.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<UsuarioLoginDTO> Logar(DTO.Request.LoginDTO loginDto);
        Task Logout();
        Task<UsuarioLoginDTO> CadastrarUsuario(CreateUserDTO createUserDto);
        Task RealizarLogin(UsuarioLoginDTO resposta);
        Task<UsuarioLoginDTO> UtilizarRefreshToken(string refreshToken);
        bool TokenExpirado();
        Task<bool> RefreshTokenValido();
        Task<UsuarioDto> GetUserDto(Guid idUsuario);
    }
}
