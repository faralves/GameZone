using GameZone.Blog.Infra.Data;
using GameZone.Core.DomainObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameZone.Blog.API.Configurations
{
    public static class DbConfig
    {
        public static void ConfigureDatabase(this WebApplicationBuilder builder)
        {
            string conectionString = builder.Configuration.GetConnectionString("Connection");

            if (GeneralConfigApp.ENABLE_CONNECTION_LOCAL_DB)
                conectionString = builder.Configuration.GetConnectionString("ConnectionLocal");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conectionString, b => b.MigrationsAssembly("GameZone.Blog.Infra"))
                                                                            .EnableSensitiveDataLogging());

        }
    }
}