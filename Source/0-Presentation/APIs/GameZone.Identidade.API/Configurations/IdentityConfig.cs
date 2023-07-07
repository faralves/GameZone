using GameZone.Core.DomainObjects;
using GameZone.Identidade.API.Extensions;
using GameZone.Identidade.Domain.Entidades;
using GameZone.Identidade.Infra;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Identidade.API.Configurations
{
    public static class IdentityConfig
    {
        public static WebApplicationBuilder AddIdentityConfiguration(this WebApplicationBuilder builder)
        {
            string conectionString = builder.Configuration.GetConnectionString("UsuarioConnection");

            if (GeneralConfigApp.ENABLE_CONNECTION_LOCAL_DB)
                conectionString = builder.Configuration.GetConnectionString("UsuarioConnectionLocal");

            builder.Services.AddDbContext<UsuarioDbContext>(options => options.UseSqlServer(conectionString, b => b.MigrationsAssembly("GameZone.Identidade.Infra")));

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
            });

            builder.Services.AddIdentity<Usuario, IdentityRole>()
                            .AddErrorDescriber<IdentityMensagensPortugues>()
                            .AddEntityFrameworkStores<UsuarioDbContext>()
                            .AddDefaultTokenProviders();

            return builder;
        }
    }
}
