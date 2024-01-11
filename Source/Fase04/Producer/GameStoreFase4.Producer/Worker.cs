using GameStoreFase4.Domain.Entities;
using GameStoreFase4.Domain.Parameters;
using GameStoreFase4.Services.Generator;
using GameStoreFase4.Services.Messages.Producer;
using Newtonsoft.Json;

namespace GameStoreFase4.Producer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IGeneratorDataService _generatorDataService;
        private readonly IProducerRabbitMqService _producerRabbitMqService;
        private readonly IProducerMassTransitService _producerMassTransitService;
        private readonly IProducerAzureServiceBusService _producerAzureServiceBusService;

        public Worker(ILogger<Worker> logger,
            IGeneratorDataService generatorDataService,
            IProducerRabbitMqService producerRabbitMqService,
            IProducerMassTransitService producerMassTransitService,
            IProducerAzureServiceBusService producerAzureServiceBusService)
        {
            _logger = logger;
            _generatorDataService = generatorDataService;
            _producerRabbitMqService = producerRabbitMqService;
            _producerMassTransitService = producerMassTransitService;
            _producerAzureServiceBusService = producerAzureServiceBusService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("\nWorker running at: {time}", DateTimeOffset.Now);
                await Process();
                await Task.Delay(5000, stoppingToken);
            }
        }


        private async Task Process()
        {
            for (int i = 1; i <= 10; i++)
            {
                Jogo jogo = _generatorDataService.Generate();
                var message = JsonConvert.SerializeObject(jogo);

                if (GlobalParameters.ENABLE_RABBIT_MQ_MESSAGE_SERVICE)
                    await _producerRabbitMqService.Publish(message, jogo.GetType(), publishDlq: false);
                else if (GlobalParameters.ENABLE_MASS_TRANSIT_MESSAGE_SERVICE)
                    await _producerMassTransitService.Publish(message, jogo.GetType(), publishDlq: false);
                else if (GlobalParameters.ENABLE_AZURE_SERVICE_BUS_MQ_MESSAGE_SERVICE)
                    await _producerAzureServiceBusService.Publish(message, jogo.GetType(), publishDlq: false);

                var logMessage = $"Jogo [{jogo.Nome.ToUpper()}] publicado na fila\n";
                _logger.LogInformation(logMessage);
            }
        }
    }
}