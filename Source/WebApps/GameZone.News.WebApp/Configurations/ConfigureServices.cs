using GameZone.Core.ConfigApp;
using GameZone.News.WebApp.Models.Services;
using GameZone.WebAPI.Core;
using Microsoft.AspNetCore.HttpOverrides;

namespace GameZone.News.WebApp.Configurations
{
    public static class ConfigureServices
    {
        private static IConfigParameters configParameters;
        public static void Configure(WebApplicationBuilder builder)
        {
            configParameters = new ConfigParameters(builder.Configuration);

            ConfigureCultureApp.ConfigureCulture(builder);

            DependencyInjectionConfig.ResolveDependencies(builder);

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }
    }
}
