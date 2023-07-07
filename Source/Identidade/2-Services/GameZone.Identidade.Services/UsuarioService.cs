using GameZone.Identidade.Domain.Entidades;
using GameZone.Identidade.Infra.Interfaces;
using GameZone.Identidade.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GameZone.Identidade.Services
{
    public class UsuarioService : IUsuarioService
    {
        private TokenService _tokenService;

        private readonly ILogger<UsuarioService> _logger;
        private IIdentidadeRepository _identidadeRepository;

        public UsuarioService(ILogger<UsuarioService> logger, TokenService tokenService, IIdentidadeRepository identidadeRepository)
        {
            _logger = logger;
            _tokenService = tokenService;
            _identidadeRepository = identidadeRepository;
        }

        public async Task<IdentityResult> CadastrarUsuario(Usuario usuario, string password)
        {
            try
            {
                return await _identidadeRepository.CadastrarUsuario(usuario, password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ocorreu erro para cadastrar o usuário.");
                throw;
            }
        }

        public async Task<string> LoginUsuario(Usuario usuario)
        {
            var usuarioDb = await _identidadeRepository.LoginUsuario(usuario);
            var token = _tokenService.GenarateToken(usuarioDb);

            return token;
        }
        public async Task<Usuario?> ObterUsuarioPorEmail(string email)
        {
            var usuarioDb = await _identidadeRepository.ObterUsuarioPorEmail(email);
            return usuarioDb;
        }
    }
}
