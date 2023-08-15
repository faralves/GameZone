using AutoMapper;
using GameZone.Core.DomainObjects;
using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Application.DTOs.Response;
using GameZone.Identidade.Application.Interfaces;
using GameZone.Identidade.Domain.Entities;
using GameZone.Identidade.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace GameZone.Identidade.Application
{
    public class UsuarioApplication : IUsuarioApplication
    {
        private IMapper _mapper;
        private readonly ILogger<UsuarioApplication> _logger;
        private IUsuarioService _usuarioService;
       

        public UsuarioApplication(IMapper mapper, ILogger<UsuarioApplication> logger, IUsuarioService usuarioService)
        {
            _mapper = mapper;
            _logger = logger;
            _usuarioService = usuarioService;
        }

        public async Task<IdentityResult> CadastrarUsuario(CreateUsuarioDto usuarioDto)
        {
            try
            {
                Usuario usuario = _mapper.Map<Usuario>(usuarioDto);
                usuario.EmailConfirmed = true;

                var resultado = await _usuarioService.CadastrarUsuario(usuario, usuarioDto.Password);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ocorreu erro para cadastrar o usuário.");
                throw;
            }
        }

        public async Task<UsuarioRespostaLogin> LoginUsuario(LoginUsuarioDto loginUsuarioDto)
        {
            try
            {
                var usuario = _mapper.Map<LoginUsuario>(loginUsuarioDto);
                return await _usuarioService.LoginUsuario(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ocorreu erro para cadastrar o usuário.");
                throw;
            }
        }

        public async Task<Usuario?> ObterUsuarioPorEmail(string email)
        {
            var usuarioDb = await _usuarioService.ObterUsuarioPorEmail(email);
            return usuarioDb;
        }

        public async Task<IList<string>> GetRolesAsync(Usuario usuario)
        {
            var roles = await _usuarioService.GetRolesAsync(usuario);
            return roles;
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId)
        {
            var role = await _usuarioService.GetRoleByIdAsync(roleId);
            return role;
        }

        public async Task<IList<Claim>> GetRoleClaimsAsync(string roleId)
        {
            var roleClaims = await _usuarioService.GetRoleClaimsAsync(roleId);
            return roleClaims;
        }

        public async Task<RefreshToken> ObterRefreshToken(Guid refreshToken)
        {
            return await _usuarioService.ObterRefreshToken(refreshToken);
        }

        public async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            return await _usuarioService.GerarJwt(email);
        }

        public async Task<UsuarioDto> GetUser(Guid idUsuario)
        {
            var usuarioDb = await _usuarioService.GetUser(idUsuario);
            var usuarioDto = _mapper.Map<UsuarioDto>(usuarioDb);

            return usuarioDto;
        }
    }
}
