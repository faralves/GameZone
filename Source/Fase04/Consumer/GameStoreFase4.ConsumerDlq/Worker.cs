using GameStoreFase4.Domain.Entities;
using GameStoreFase4.Services.Messages.Consumer;

namespace GameStoreFase4.ConsumerDlq;
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
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Process();
            await Task.Delay(90000, stoppingToken);
        }
    }

    private async Task Process()
    {
        _logger.LogInformation("\nProcessando Consumer Dlq");
        ObjectMessageProcessedInfo result = _consumerRabbitMqService.Consume(consumeDlq: true);
        _logger.LogInformation("\nConsumer Dlq Processado");
    }
}