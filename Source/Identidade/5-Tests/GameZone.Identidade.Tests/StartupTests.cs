using GameZone.Core.DomainObjects;
using GameZone.Identidade.API.Extensions;
using GameZone.Identidade.Domain.Entities;
using GameZone.Identidade.Infra;
using GameZone.Identidade.Tests.Api.Infra;
using GameZone.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.Security.Jwt.Core.Jwa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Identidade.Tests.Api
{
    public class StartupTests
    {
        private readonly DockerFixture _dockerFixture;

        public StartupTests(DockerFixture dockerFixture)
        {
            _dockerFixture = dockerFixture;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAspNetUser, AspNetUser>();


            services.AddJwksManager(options => options.Jws = Algorithm.Create(AlgorithmType.ECDsa, JwtType.Jws))
                .UseJwtValidation()
                .PersistKeysToDatabaseStore<UsuarioDbContext>();

            services.AddAuthorization();
            services.AddMemoryCache();

            services.AddDbContext<UsuarioDbContext>(options => options.UseSqlServer(_dockerFixture.GetConnectionString(), b => b.MigrationsAssembly("GameZone.Identidade.Infra")));

            services.Configure<IdentityOptions>(options =>
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


            services.AddIdentity<Usuario, IdentityRole>()
                            .AddErrorDescriber<IdentityMensagensPortugues>()
                            .AddEntityFrameworkStores<UsuarioDbContext>()
                            .AddDefaultTokenProviders();
        }
    }
}
