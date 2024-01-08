using GameStoreFase4.Infrastructure.Repositories;
using GameStoreFase4.Services.File;
using GameStoreFase4.Services.Generator;
using GameStoreFase4.Services.Messages.Consumer;
using GameStoreFase4.Services.Messages.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Reflection;

namespace GameStoreFase4.IoC;
public static class Startup
{
    public static void Configure(IConfiguration configuration, IServiceCollection services, bool enableSwagger = false)
    {
        services.AddSingleton<IJogoRepository, JogoRepository>();
        services.AddSingleton<IFileManager, FileManager>();
        services.AddSingleton<IGeneratorDataService, GeneratorDataService>();

        ConfigureRabbitMq(configuration, services);
        if (enableSwagger)
            ConfigureSwagger(services);
    }
    private static void ConfigureRabbitMq(IConfiguration configuration, IServiceCollection services)
    {
        string rabbitConnStr = configuration.GetConnectionString("ServerRabbitMQ");

        var rabbitMqFactory = new ConnectionFactory
        {
            Uri = new Uri(rabbitConnStr)
        };

        var rabbitMqConnection = rabbitMqFactory.CreateConnection();
        var rabbitMqChannel = rabbitMqConnection.CreateModel();

        var jogoRepository = new JogoRepository(configuration);
        var producerService = new ProducerRabbitMqService(configuration, rabbitMqChannel);

        IFileManager fileManager = new FileManager(configuration);

        services.AddSingleton<IProducerRabbitMqService, ProducerRabbitMqService>(sp =>
        {
            return producerService;
        });

        services.AddSingleton<IConsumerRabbitMqService, ConsumerRabbitMqService>(sp =>
        {
            return new ConsumerRabbitMqService(configuration, rabbitMqChannel, producerService, jogoRepository, fileManager);
        });
    }
    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "API Produtor RabbitMq",
                Description = "API Produtor de mensagens para RabbitMQ",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Andre Silva",
                    Email = "andreleaos@gmail.com",
                    Url = new Uri("https://twitter.com/seuhandle")
                },
                License = new OpenApiLicense
                {
                    Name = "Licença",
                    Url = new Uri("https://example.com/license")
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            if (AppContext.BaseDirectory.Contains("GameStoreFase4.Api"))
                xmlFile = xmlFile.Replace("IoC", "Api");

            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }

}