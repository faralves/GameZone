using GameZone.Core.ConfigApp;
using GameZone.WebAPI.Core;

namespace GameZone.Identidade.API.Configurations
{
    public static class ConfigureServices
    {
        private static IConfigParameters configParameters;

        public static void Configure(WebApplicationBuilder builder)
        {
            ConfigureAppSetting.ConfigureUseAppSetting(builder);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyOrigin()  // Permite qualquer origem
                        .AllowAnyMethod()  // Permite qualquer método HTTP (GET, POST, etc.)
                        .AllowAnyHeader(); // Permite qualquer cabeçalho HTTP
                });
            });

            configParameters = new ConfigParameters(builder.Configuration);

            configParameters.SetGeneralConfig();

            IdentityConfig.AddIdentityConfiguration(builder);

            ConfigureCultureApp.ConfigureCulture(builder);

            DependencyInjectionConfig.ResolveDependencies(builder);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            JwtConfig.AddJwtConfiguration(builder);

            SwaggerConfig.ConfigureSwagger(builder);
        }
    }
}
