using GameStoreFase4.Domain.Entities;
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

        public Worker(ILogger<Worker> logger, IGeneratorDataService generatorDataService, IProducerRabbitMqService producerRabbitMqService)
        {
            _logger = logger;
            _generatorDataService = generatorDataService;
            _producerRabbitMqService = producerRabbitMqService;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("\nWorker running at: {time}", DateTimeOffset.Now);

                for (int i = 1; i <= 10; i++)
                {
                    Jogo jogo = _generatorDataService.Generate();
                    var message = JsonConvert.SerializeObject(jogo);
                    await _producerRabbitMqService.Publish(message, jogo.GetType(), publishDlq: false);

                    var logMessage = $"Jogo [{jogo.Nome.ToUpper()}] publicado na fila\n";
                    _logger.LogInformation(logMessage);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}