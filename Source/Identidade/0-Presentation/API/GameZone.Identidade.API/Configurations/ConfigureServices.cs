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

            configParameters = new ConfigParameters(builder.Configuration);

            configParameters.SetGeneralConfig();

            IdentityConfig.AddIdentityConfiguration(builder);

            ConfigureCultureApp.ConfigureCulture(builder);

            DependencyInjectionConfig.ResolveDependencies(builder);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //JwtConfig.ConfigureAuthorize(builder);
            JwtConfig.AddJwtConfiguration(builder);

            SwaggerConfig.ConfigureSwagger(builder);
        }
    }
}
