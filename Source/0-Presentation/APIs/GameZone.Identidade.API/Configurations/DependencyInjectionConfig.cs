using GameZone.Identidade.API.Authorization;
using GameZone.Identidade.API.Configurations.Interfaces;
using GameZone.Identidade.Application;
using GameZone.Identidade.Application.Interfaces;
using GameZone.Identidade.Infra.Interfaces;
using GameZone.Identidade.Infra.Repository;
using GameZone.Identidade.Services;
using GameZone.Identidade.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace GameZone.Identidade.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder ResolveDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IUsuarioApplication, UsuarioApplication>();
            builder.Services.AddScoped<IIdentidadeRepository, IdentidadeRepository>();
            builder.Services.AddScoped<ISeed, Seed>();
            builder.Services.AddScoped<TokenService>();

            builder.Services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>();

            return builder;
        }
    }
}
