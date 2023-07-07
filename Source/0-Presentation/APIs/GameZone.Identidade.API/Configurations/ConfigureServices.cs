using GameZone.Core.ConfigApp;
using GameZone.Identidade.API.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

namespace GameZone.Identidade.API.Configurations
{
    public static class ConfigureServices
    {
        private static IConfigParameters configParameters;

        public static void Configure(WebApplicationBuilder builder)
        {
            configParameters = new ConfigParameters(builder.Configuration);

            ConfigureDatabase(builder.Configuration);
            IdentityConfig.AddIdentityConfiguration(builder);
            ConfigureCulture(builder);
            DependencyInjectionConfig.ResolveDependencies(builder);
            ConfigureAutoMapper(builder);
            ConfigureAuthorize(builder);
            ConfigureSwagger(builder);
        }

        private static void ConfigureCulture(this WebApplicationBuilder builder)
        {
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            var cultureInfo = builder.Configuration["CultureInfo"];
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(cultureInfo);
                options.SupportedCultures = new[] { new CultureInfo(cultureInfo) };
                options.SupportedUICultures = new[] { new CultureInfo(cultureInfo) };
            });
        }

        private static void ConfigureDatabase(IConfiguration configuration)
        {
            configParameters.SetGeneralConfig();
        }

        private static void ConfigureAutoMapper(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }


        private static void ConfigureAuthorize(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SymmetricSecurityKey"])),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("IdadeMinima", policy =>
                     policy.AddRequirements(new IdadeMinima(Convert.ToInt32(builder.Configuration["idadeMinima"])))
                );
            });
        }

        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            // Configurar o Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Usuários", Version = "v1" });

                // Adicionar o esquema de segurança JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Cabeçalho de autorização JWT usando o esquema Bearer",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // Adicionar o requisito de segurança para todas as operações do Swagger
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }
    }
}
