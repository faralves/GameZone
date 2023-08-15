using Microsoft.AspNetCore.Authentication.Cookies;

namespace GameZone.News.WebApp.Configurations
{
    public static class IdentityConfig
    {
        public static void AddIdentityConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/logar";
                    options.AccessDeniedPath = "/erro/403";
                });
        }
    }
}
