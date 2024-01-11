using GameStoreFase4.IoC;

namespace GameStoreFase4.ParametersApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Startup.Configure(builder.Configuration, builder.Services, enableSwagger: true, enableRabbitMq: false, apiSwagger: "parameter");


            builder.Services.AddControllers();

            var app = builder.Build();
            ConfigureSwagger(app);

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureSwagger(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gestão de Serviços Mensageria");
                // c.RoutePrefix = string.Empty;
            });
        }

    }
}