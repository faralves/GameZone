using AutoMapper;
using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Application.Interfaces;
using GameZone.Identidade.Domain.Entidades;
using GameZone.Identidade.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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

        public async Task<string> LoginUsuario(LoginUsuarioDto loginUsuarioDto)
        {
            try
            {
                Usuario usuario = _mapper.Map<Usuario>(loginUsuarioDto);
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

    }
}
