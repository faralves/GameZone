using GameZone.Core.ConfigApp;
using GameZone.WebAPI.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GameZone.Blog.API.Configurations
{
    public static class ConfigureServices
    {
        private static IConfigParameters configParameters;

        public static void Configure(WebApplicationBuilder builder)
        {
            ConfigureAppSetting.ConfigureUseAppSetting(builder);

            configParameters = new ConfigParameters(builder.Configuration);

            configParameters.SetGeneralConfig();

            ConfigureCultureApp.ConfigureCulture(builder);

            DependencyInjectionConfig.ResolveDependencies(builder);

            DbConfig.ConfigureDatabase(builder);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            JwtConfig.AddJwtConfiguration(builder);

            SwaggerConfig.ConfigureSwagger(builder);
        }
    }
}
