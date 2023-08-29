using GameZone.News.WebApp.Models.Interfaces;
using GameZone.News.WebApp.Models.Services;
using GameZone.News.WebApp.Models.Services.Handlers;
using GameZone.WebAPI.Core.Extensions;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly;
using GameZone.WebAPI.Core.Usuario;

namespace GameZone.News.WebApp.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder ResolveDependencies(this WebApplicationBuilder builder)
        {
            IdentityConfig.AddIdentityConfiguration(builder);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();

            //builder.Services.AddDataProtection()
            //    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var/data_protection_keys/"))
            //    .SetApplicationName("GameZone");

            builder.Services.AddScoped<IAspNetUser, AspNetUser>();

            builder.Services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            builder.Services.AddHttpClient<INewsService, NewsService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AllowSelfSignedCertificate();
                //.AddPolicyHandler(PollyExtensions.EsperarTentar())
                //.AddTransientHttpErrorPolicy(
                //    p => p.CircuitBreakerAsync(8, TimeSpan.FromSeconds(60)));

            builder.Services.AddHttpClient<IAutenticacaoService, AutenticacaoService>()
                .AllowSelfSignedCertificate();
                //.AddPolicyHandler(PollyExtensions.EsperarTentar())
                //.AddTransientHttpErrorPolicy(
                //    p => p.CircuitBreakerAsync(8, TimeSpan.FromSeconds(60)));

            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddControllersWithViews();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return builder;
        }
    }

    #region PollyExtension
    public static class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
        {
            var retry = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                }, (outcome, timespan, retryCount, context) =>
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Tentando pela {retryCount} vez!");
                    Console.ForegroundColor = ConsoleColor.White;
                });

            return retry;
        }
    }

    #endregion

}
