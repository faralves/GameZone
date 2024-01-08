using GameStoreFase4.Domain.Entities;
using GameStoreFase4.Services.Messages.Consumer;

namespace GameStoreFase4.Consumer;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConsumerRabbitMqService _consumerRabbitMqService;

    public Worker(ILogger<Worker> logger,
        IConsumerRabbitMqService consumerRabbitMqService)
    {
        _logger = logger;
        _consumerRabbitMqService = consumerRabbitMqService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("\nBackground Service em execucao no horario: {time}\n", DateTimeOffset.Now);

            _logger.LogInformation("\nProcessando Consumer");
            ObjectMessageProcessedInfo result = _consumerRabbitMqService.Consume(consumeDlq: false);
            _logger.LogInformation("\nConsumer Processado");

            await Task.Delay(50000, stoppingToken);
        }
    }
}