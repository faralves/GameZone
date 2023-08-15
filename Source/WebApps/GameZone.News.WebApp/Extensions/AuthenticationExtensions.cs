using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GameZone.News.WebApp.Extensions
{
    public static class AuthenticationExtensions
    {
        public static async Task SignInWithTokenAsync(this HttpContext httpContext, string token)
        {
            var claims = new List<Claim>
            {
                new Claim("JWT", token)
                // Adicione outras claims necessárias para o usuário, se houver
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                // Configurar as propriedades do cookie de autenticação, se necessário
            };

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }

}
