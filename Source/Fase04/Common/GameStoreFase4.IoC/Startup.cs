using GameStoreFase4.Infrastructure.Repositories;
using GameStoreFase4.Services.File;
using GameStoreFase4.Services.Generator;
using GameStoreFase4.Services.Messages.Consumer;
using GameStoreFase4.Services.Messages.Producer;
using GameStoreFase4.Services.Parameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Reflection;

namespace GameStoreFase4.IoC;
public static class Startup
{
    public static void Configure(IConfiguration configuration, IServiceCollection services, bool enableSwagger = false, bool enableRabbitMq = false, string apiSwagger = "")
    {
        ConfigureRepositories(services);
        ConfigureServices(services);

        if (enableRabbitMq)
            ConfigureRabbitMq(configuration, services);
        if (enableSwagger)
            ConfigureSwagger(services, apiSwagger);

        ConfigureMassTransit(configuration, services);
    }
    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddSingleton<IJogoRepository, JogoRepository>();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IFileManagerService, FileManagerService>();
        services.AddSingleton<IGeneratorDataService, GeneratorDataService>();

        services.AddSingleton<IConsumerMassTransitService, ConsumerMassTransitService>();
        services.AddSingleton<IConsumerAzureServiceBusService, ConsumerAzureServiceBusService>();

        services.AddSingleton<IProducerMassTransitService, ProducerMassTransitService>();
        services.AddSingleton<IProducerAzureServiceBusService, ProducerAzureServiceBusService>();

        services.AddSingleton<IParameterService, ParameterService>();
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

        IFileManagerService fileManager = new FileManagerService(configuration);

        services.AddSingleton<IProducerRabbitMqService, ProducerRabbitMqService>(sp =>
        {
            return producerService;
        });

        services.AddSingleton<IConsumerRabbitMqService, ConsumerRabbitMqService>(sp =>
        {
            return new ConsumerRabbitMqService(configuration, rabbitMqChannel, producerService, jogoRepository, fileManager);
        });
    }
    private static void ConfigureSwagger(IServiceCollection services, string apiSwagger = "")
    {
        string title = "";
        string description = "";

        if (apiSwagger == "producer")
        {
            title = "API Produtor de Mensagens";
            description = "API Produtor de mensagens para serviço mensageria";
        }
        else if (apiSwagger == "parameter")
        {
            title = "API Gestão de Serviços Mensageria";
            description = "API Gestão de Serviços Mensageria";
        }
        else
        {
            title = "APIde Serviços";
            description = "API Gestão de Serviços";
        }

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = title,
                Description = description,
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
        });
    }
    private static void ConfigureMassTransit(IConfiguration configuration, IServiceCollection services)
    {

    }

}