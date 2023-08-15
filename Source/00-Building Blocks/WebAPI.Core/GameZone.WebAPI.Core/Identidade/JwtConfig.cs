using GameZone.WebAPI.Core.Identidade;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.JwtExtensions;
using System.Text;

namespace GameZone.WebAPI.Core
{
    public static class JwtConfig
    {
        public static void ConfigureAuthorize(WebApplicationBuilder builder)
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

            // Configurar a autenticação com cookies
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).AddCookie();

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("IdadeMinima", policy =>
            //         policy.AddRequirements(new IdadeMinima(Convert.ToInt32(builder.Configuration["idadeMinima"])))
            //    );
            //});
        }

        public static void AddJwtConfiguration(WebApplicationBuilder builder)
        {
            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            //.AddCookie(options =>
            //{
            //    //options.LoginPath = "/logar";
            //    //options.AccessDeniedPath = "/erro/403";
            //    //options.Cookie.Name = "GameZone";
            //    //options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromHours(8); ; // Configure o tempo de expiração adequado
            //    options.SlidingExpiration = true;
            //})
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                x.SaveToken = true;
                x.SetJwksOptions(new JwkOptions(appSettings.AutenticacaoJwksUrl));
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // Defina como true se quiser validar o emissor (issuer) do token
                    ValidateAudience = false // Defina como true se quiser validar a audiência (audience) do token
                };
            });
        }

        public static void UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}