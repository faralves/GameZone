using GameZone.Core.DomainObjects;
using GameZone.Identidade.API.Extensions;
using GameZone.Identidade.Domain;
using GameZone.Identidade.Domain.Entities;
using GameZone.Identidade.Infra;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Jwa;

namespace GameZone.Identidade.API.Configurations
{
    public static class IdentityConfig
    {
        public static WebApplicationBuilder AddIdentityConfiguration(this WebApplicationBuilder builder)
        {
            var appSettingsSection = builder.Configuration.GetSection("AppTokenSettings");
            builder.Services.Configure<AppTokenSettings>(appSettingsSection);
            
            builder.Services.AddJwksManager(options => options.Jws = Algorithm.Create(AlgorithmType.ECDsa, JwtType.Jws))
                .UseJwtValidation()
                .PersistKeysToDatabaseStore<UsuarioDbContext>();

            builder.Services.AddAuthorization();
            builder.Services.AddMemoryCache();

            string conectionString = builder.Configuration.GetConnectionString("Connection");

            if (GeneralConfigApp.ENABLE_CONNECTION_LOCAL_DB)
                conectionString = builder.Configuration.GetConnectionString("ConnectionLocal");

            builder.Services.AddDbContext<UsuarioDbContext>(options => options.UseSqlServer(conectionString, b => b.MigrationsAssembly("GameZone.Identidade.Infra")));

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Configurações de senha
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;

                // Configurações de bloqueio de conta
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // ...

                // Configurações de token
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            });

            builder.Services.AddIdentity<Usuario, IdentityRole>()
                            .AddErrorDescriber<IdentityMensagensPortugues>()
                            .AddEntityFrameworkStores<UsuarioDbContext>()
                            .AddDefaultTokenProviders();

            //builder.Services.AddDefaultIdentity<IdentityUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddErrorDescriber<IdentityMensagensPortugues>()
            //    .AddEntityFrameworkStores<UsuarioDbContext>()
            //    .AddDefaultTokenProviders();

            return builder;
        }
    }
}
