using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using GameZone.Blog.Infra.Data;
using GameZone.Core.DomainObjects;
using GameZone.Core.ConfigApp;

namespace GameZone.Blog.API.Configurations
{
    public static class ConfigureServices
    {
        private static IConfigParameters configParameters;
        public static void Configure(WebApplicationBuilder builder)
        {
            configParameters = new ConfigParameters(builder.Configuration);

            ConfigureCulture(builder);
            DependencyInjectionConfig.ResolveDependencies(builder);
            ConfigureDatabase(builder);
            ConfigureAutoMapper(builder);
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

        private static void ConfigureDatabase(this WebApplicationBuilder builder)
        {
            configParameters.SetGeneralConfig();

            string conectionString = builder.Configuration.GetConnectionString("UsuarioConnection");

            if (GeneralConfigApp.ENABLE_CONNECTION_LOCAL_DB)
                conectionString = builder.Configuration.GetConnectionString("UsuarioConnectionLocal");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conectionString, b => b.MigrationsAssembly("GameZone.Blog.Infra"))
                                                                            .EnableSensitiveDataLogging());
        }

        private static void ConfigureAutoMapper(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            // Configurar o Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API para o Blog de Games", Version = "v1" });

                // Adicionar o esquema de segurança JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"'Bearer' [space] seu token",
                    //Type = SecuritySchemeType.Http,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
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
