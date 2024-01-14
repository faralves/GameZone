using GameStoreFase4.Domain.Entities;
using GameStoreFase4.Infrastructure.Repositories;
using GameStoreFase4.Services.File;
using Newtonsoft.Json;

namespace GameStoreFase4.ConsumerFile;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IFileManagerService _fileManager;
    private readonly IJogoRepository _jogoRepository;

    public Worker(ILogger<Worker> logger,
        IFileManagerService fileManager,
        IJogoRepository jogoRepository)
    {
        _logger = logger;
        _fileManager = fileManager;
        _jogoRepository = jogoRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Process();
            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task Process()
    {
        var messages = _fileManager.GetMessages();

        if (messages is null)
            return;

        var jogos = new List<Jogo>();
        foreach (var item in messages)
        {
            Jogo jogo = JsonConvert.DeserializeObject<Jogo>(item);
            jogos.Add(jogo);
        }

        _jogoRepository.Cadastrar(jogos);

        var jogosPersistidos = new List<string>();
        foreach (var item in jogos)
        {
            var serializedItem = JsonConvert.SerializeObject(item);
            jogosPersistidos.Add(serializedItem);
        }

        _fileManager.Save(jogosPersistidos, salvoEmBD: true);
        _fileManager.CleanDlqFile();
    }
}