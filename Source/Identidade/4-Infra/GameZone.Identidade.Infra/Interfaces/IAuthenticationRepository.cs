using GameZone.Identidade.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Identidade.Infra.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<UsuarioRespostaLogin> GerarJwt(string email);
        Task<RefreshToken> ObterRefreshToken(Guid refreshToken);
    }
}
