using Microsoft.AspNetCore.Authentication.Cookies;

namespace GameZone.News.WebApp.Configurations
{
    public static class IdentityConfig
    {
        public static void AddIdentityConfiguration(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/logar";
                    options.AccessDeniedPath = "/erro/403";
                    options.Cookie.Name = "GameZone";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Configure o tempo de expiração adequado
                    options.SlidingExpiration = true;
                });
        }
    }
}
