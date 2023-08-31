using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GameZone.Core.Communication;
using GameZone.News.WebApp.Models.DTO.Request;
using GameZone.News.WebApp.Models.DTO.Response;
using GameZone.News.WebApp.Models.Interfaces;
using GameZone.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace GameZone.News.WebApp.Models.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        private readonly IAuthenticationService _authenticationService;
        private readonly IAspNetUser _user;

        private readonly IConfiguration _configuration;

        private string _url_login_address = string.Empty;
        private string _url_usuario_address = string.Empty;
        private string _url_refresh_token_address = string.Empty;

        public AutenticacaoService(IAuthenticationService authenticationService, IConfiguration configuration, IAspNetUser user, HttpClient httpClient)
        {
            //try
            //{
                _authenticationService = authenticationService;

                _configuration = configuration;

                _httpClient = httpClient;
                string urlAddress = _configuration.GetSection("AutenticacaoUrl").Value;
                httpClient.BaseAddress = new Uri(urlAddress);

                _url_login_address = $"{urlAddress}/Usuario/login";
                _url_refresh_token_address = $"{urlAddress}/Usuario/refresh-token";
                _url_usuario_address += $"{urlAddress}/Usuario";

                _user = user;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task Logout()
        {
            //try
            //{
                await _authenticationService.SignOutAsync(_user.ObterHttpContext(), CookieAuthenticationDefaults.AuthenticationScheme, null);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task<UsuarioLoginDTO> Logar(DTO.Request.LoginDTO loginDto)
        {
            //try
            //{
                UsuarioLoginDTO usuarioLoginDTO = null;
                
                var loginContent = ObterConteudo(loginDto);

                using (var response = await _httpClient.PostAsync(_url_login_address, loginContent))
                {
                    if (response.IsSuccessStatusCode)
                        usuarioLoginDTO = await response.Content.ReadFromJsonAsync<DTO.Response.UsuarioLoginDTO>();
                }

                return usuarioLoginDTO;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task RealizarLogin(UsuarioLoginDTO resposta)
        {
            //try
            //{
                var token = ObterTokenFormatado(resposta.AccessToken);

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, resposta.UsuarioToken.Email));
                claims.Add(new Claim("JWT", resposta.AccessToken));
                claims.Add(new Claim("RefreshToken", resposta.RefreshToken));
                claims.AddRange(token.Claims);

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                    IsPersistent = true,
                    AllowRefresh = true
                };

                var httpContext = _user.ObterHttpContext();

                var principal = new ClaimsPrincipal(claimsIdentity);

                await _authenticationService.SignInAsync(
                    httpContext,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProperties);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            //try
            //{
                return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task<UsuarioLoginDTO> CadastrarUsuario(CreateUserDTO createUserDto)
        {
            //try
            //{
                UsuarioLoginDTO usuarioLoginDto = null;

                var userContent = ObterConteudo(createUserDto);
                string endpoint = $"{_url_usuario_address}/cadastrar";

                using (var response = await _httpClient.PostAsync(endpoint, userContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var loginDto = new DTO.Request.LoginDTO() { Email = createUserDto.Email, Password = createUserDto.Password };
                        usuarioLoginDto = await Logar(loginDto);
                    }
                }

                return usuarioLoginDto;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public bool TokenExpirado()
        {
            //try
            //{
                var jwt = _user.ObterUserToken();
                if (jwt is null) return false;

                var token = ObterTokenFormatado(jwt);
                return token.ValidTo.ToLocalTime() < DateTime.Now;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task<bool> RefreshTokenValido()
        {
            //try
            //{
                var resposta = await UtilizarRefreshToken(_user.ObterUserRefreshToken());

                if (resposta.AccessToken != null && resposta.ResponseResult == null)
                {
                    await RealizarLogin(resposta);
                    return true;
                }

                return false;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task<UsuarioLoginDTO> UtilizarRefreshToken(string refreshToken)
        {
            //try
            //{
                UsuarioLoginDTO usuarioLoginDTO = null;
                var refreshTokenContent = ObterConteudo(refreshToken);

                using (var response = await _httpClient.PostAsync(_url_refresh_token_address, refreshTokenContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        usuarioLoginDTO = await DeserializarObjetoResponse<UsuarioLoginDTO>(response);
                    }
                    else
                    {
                        usuarioLoginDTO = new UsuarioLoginDTO()
                        {
                            ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                        };
                    }
                }

                return usuarioLoginDTO;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task<UsuarioDto> GetUserDto(Guid idUsuario)
        {
            //try
            //{
                UsuarioDto usuarioDto = new UsuarioDto(); 
                string endpoint = $"{_url_usuario_address}/{idUsuario.ToString()}";

                using (var response = await _httpClient.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var usuarioJson = await response.Content.ReadAsStringAsync();
                        usuarioDto = JsonConvert.DeserializeObject<UsuarioDto>(usuarioJson);
                    }
                }

                return usuarioDto;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
    }
}
