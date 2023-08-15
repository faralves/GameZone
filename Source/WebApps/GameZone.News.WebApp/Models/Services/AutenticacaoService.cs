﻿using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
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
        private readonly HttpClient _httpClientUsuario;

        private readonly IAuthenticationService _authenticationService;
        private readonly IAspNetUser _user;

        private readonly IConfiguration _configuration;

        private string _url_login_address = string.Empty;
        private string _url_usuario_address = string.Empty;
        private string _url_refresh_token_address = string.Empty;

        public AutenticacaoService(IAuthenticationService authenticationService, IConfiguration configuration, IAspNetUser user, HttpClient httpClient, HttpClient httpClientUsuario)
        {
            _authenticationService = authenticationService;

            _configuration = configuration;
            _url_login_address = _configuration.GetSection("AutenticacaoUrl").Value;
            _url_login_address += "/Usuario/login";
            _url_refresh_token_address += "/Usuario/refresh-token";

            httpClient.BaseAddress = new Uri(_url_login_address);
            _httpClient = httpClient;

            _user = user;

            _url_usuario_address = _configuration.GetSection("AutenticacaoUrl").Value;
            _url_usuario_address += "/Usuario";
            httpClientUsuario.BaseAddress = new Uri(_url_usuario_address);
            _httpClientUsuario = httpClientUsuario;
        }

        private async Task<HttpClient> GetHttpClientAsync(bool PrecisaToken)
        {
            var token = _user.ObterUserToken();

            if (PrecisaToken)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return _httpClient;
        }

        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(
                _user.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                null);
        }

        public async Task<UsuarioLoginDTO> Logar(DTO.Request.LoginDTO loginDto)
        {
            var client = await GetHttpClientAsync(false);
            var loginContent = ObterConteudo(loginDto);
            string endpoint = $"{_url_login_address}";

            var response = await client.PostAsync(endpoint, loginContent);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<DTO.Response.UsuarioLoginDTO>();

            UsuarioLoginDTO usuarioLoginDTO = null;
            return usuarioLoginDTO;
        }

        public async Task RealizarLogin(UsuarioLoginDTO resposta)
        {
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
            //httpContext.User = principal;

            //await _authenticationService.SignInAsync(
            //    httpContext,
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    principal,
            //    authProperties);

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);


        }

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

        public async Task<UsuarioLoginDTO> CadastrarUsuario(CreateUserDTO createUserDto)
        {
            var client = await GetHttpClientAsync(false);
            var userContent = ObterConteudo(createUserDto);
            string endpoint = $"{_url_login_address}";

            var response = await client.PostAsync(endpoint, userContent);
            if (response.IsSuccessStatusCode)
            {
                var loginDto = new DTO.Request.LoginDTO() { Email = createUserDto.Email, Password = createUserDto.Password };
                return await Logar(loginDto);
            }
            UsuarioLoginDTO usuarioLoginDto = null;
            return usuarioLoginDto;
        }

        public bool TokenExpirado()
        {
            var jwt = _user.ObterUserToken();
            if (jwt is null) return false;

            var token = ObterTokenFormatado(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<bool> RefreshTokenValido()
        {
            var resposta = await UtilizarRefreshToken(_user.ObterUserRefreshToken());

            if (resposta.AccessToken != null && resposta.ResponseResult == null)
            {
                await RealizarLogin(resposta);
                return true;
            }

            return false;
        }

        public async Task<UsuarioLoginDTO> UtilizarRefreshToken(string refreshToken)
        {
            var client = await GetHttpClientAsync(false);
            var refreshTokenContent = ObterConteudo(refreshToken);
            string endpoint = $"{_url_refresh_token_address}";

            var response = await client.PostAsync(endpoint, refreshTokenContent);

            if (response.IsSuccessStatusCode)
            {
                return await DeserializarObjetoResponse<UsuarioLoginDTO>(response);
            }
            else
            {
                return new UsuarioLoginDTO
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }
        }

        public async Task<UsuarioDto> GetUserDto(Guid idUsuario)
        {
            string endpoint = $"{_url_usuario_address}/{idUsuario.ToString()}";
            var response = await _httpClientUsuario.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var usuarioJson = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<UsuarioDto>(usuarioJson);
                return usuario;
            }
            else
            {
                return new UsuarioDto();
            }

        }
    }
}
