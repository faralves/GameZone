using GameZone.Blog.Infra.Repository;
using GameZone.Blog.Application;
using GameZone.Blog.Application.Interfaces;
using GameZone.Blog.Infra.Interfaces;
using GameZone.Blog.Services;
using GameZone.Blog.Services.Interfaces;

namespace GameZone.Blog.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder ResolveDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<INoticiaApplication, NoticiaApplication>();
            builder.Services.AddScoped<INoticiaService, NoticiaService>();
            builder.Services.AddScoped<INoticiaRepository, NoticiaRepository>();

            return builder;
        }
    }
}
