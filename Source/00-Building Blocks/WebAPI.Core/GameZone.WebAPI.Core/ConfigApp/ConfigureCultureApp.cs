using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace GameZone.WebAPI.Core
{
    public static class ConfigureCultureApp
    {
        public static void ConfigureCulture(this WebApplicationBuilder builder)
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
    }
}