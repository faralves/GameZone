using GameStoreFase4.Domain.Entities;
using GameStoreFase4.Infrastructure.Repositories;
using GameStoreFase4.Services.File;
using GameStoreFase4.Services.Messages.Producer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace GameStoreFase4.Services.Messages.Consumer;
public class ConsumerRabbitMqService : IConsumerRabbitMqService
{
    private readonly IModel _channel;
    private readonly IProducerRabbitMqService _producerRabbitMqService;
    private readonly IJogoRepository _jogoRepository;
    private readonly IFileManagerService _fileManager;

    private static string _queuename = null;
    private static string _queueDlqName = null;

    private Jogo _currentObject;
    private bool _processedSuccessfully = false;

    public ConsumerRabbitMqService(
        IConfiguration configuration,
        IModel channel,
        IProducerRabbitMqService producerRabbitMqService,
        IJogoRepository jogoRepository,
        IFileManagerService fileManager)
    {
        _channel = channel;
        _producerRabbitMqService = producerRabbitMqService;
        _jogoRepository = jogoRepository;
        _fileManager = fileManager;
        _queuename = configuration["RabbitMqConfig:Queue"];
        _queueDlqName = configuration["RabbitMqConfig:QueueDlq"];
    }


    public ObjectMessageProcessedInfo Consume(bool consumeDlq = false)
    {
        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

        string queuename = consumeDlq ? _queueDlqName : _queuename;
        consumer.Received += Consumer_Message;
        _channel.BasicConsume(queuename, false, consumer);

        return new ObjectMessageProcessedInfo
        {
            Object = _currentObject,
            ProcessedSuccessfully = _processedSuccessfully,
            ProcessedDlqQueue = consumeDlq
        };
    }

    private void Consumer_Message(object? sender, BasicDeliverEventArgs e)
    {
        bool processouComSucesso = false;
        string messageContent = Encoding.UTF8.GetString(e.Body.ToArray());
        Jogo jogo = null;

        if (!string.IsNullOrEmpty(messageContent))
        {
            Console.WriteLine($"Conteudo da mensagem: {messageContent}");
            try
            {
                jogo = JsonConvert.DeserializeObject<Jogo>(messageContent);
                _currentObject = jogo ?? null;

                if (jogo != null && (jogo.Id > 0 || !string.IsNullOrEmpty(jogo.Nome)))
                {
                    if (jogo.Id == 0 && !string.IsNullOrEmpty(jogo.Nome))
                    {
                        _jogoRepository.Cadastrar(jogo);
                    }
                    else if (jogo.Id > 0 && !string.IsNullOrEmpty(jogo.Nome))
                    {
                        _jogoRepository.Atualizar(jogo);
                    }
                    else if (jogo.Id > 0)
                    {
                        _jogoRepository.Excluir(jogo.Id);
                    }

                    processouComSucesso = true;
                    _processedSuccessfully = true;
                    Console.WriteLine($"Jogo processado com sucesso - [{jogo.Nome.ToUpper()}]\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Falha no tratamento da mensagem] - {ex.Message}");

                if (jogo != null)
                {
                    string message = JsonConvert.SerializeObject(jogo);

                    if (!e.RoutingKey.ToLower().Contains("dlq"))
                        _producerRabbitMqService.Publish(message, jogo.GetType(), publishDlq: true);
                    else if (e.RoutingKey.ToLower().Contains("dlq"))
                        _fileManager.Save(message);

                    processouComSucesso = true;
                }
            }
            finally
            {
                if (processouComSucesso)
                    _channel.BasicAck(e.DeliveryTag, false);
                else
                    _channel.BasicNack(e.DeliveryTag, false, false);
            }
        }
    }

}