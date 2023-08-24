using GameZone.Identidade.API.Authorization;
using GameZone.Identidade.API.Configurations.Interfaces;
using GameZone.Identidade.Application;
using GameZone.Identidade.Application.Interfaces;
using GameZone.Identidade.Infra.Interfaces;
using GameZone.Identidade.Infra.Repository;
using GameZone.Identidade.Services;
using GameZone.Identidade.Services.Interfaces;
using GameZone.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using NetDevPack.Security.Jwt.Core.Interfaces;

namespace GameZone.Identidade.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder ResolveDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUsuarioApplication, UsuarioApplication>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IIdentidadeRepository, IdentidadeRepository>();
            builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            builder.Services.AddScoped<IAspNetUser, AspNetUser>();

            builder.Services.AddScoped<ISeed, Seed>();

            builder.Services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>();

            return builder;
        }
    }
}
