using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace GameStoreFase4.Services.Messages.Producer;
public class ProducerRabbitMqService : IProducerRabbitMqService
{
    private readonly IModel _channel;
    private static string _exchangeName = null;
    private static string _routingKey = null;
    private static string _routingKeyDlq = null;

    public ProducerRabbitMqService(IConfiguration configuration, IModel channel)
    {
        _channel = channel;
        _exchangeName = configuration["RabbitMqConfig:Exchange"];
        _routingKey = configuration["RabbitMqConfig:RoutingKey"];
        _routingKeyDlq = configuration["RabbitMqConfig:RoutingKeyDlq"];
    }

    public async Task Publish(string message, Type objectType, bool publishDlq = false)
    {
        object objDeserialized = JsonConvert.DeserializeObject(message, objectType);
        var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objDeserialized));

        string routingKey = publishDlq ? _routingKeyDlq : _routingKey;

        _channel.BasicPublish(
            exchange: _exchangeName,
            routingKey: routingKey,
            false,
            basicProperties: null,
            body: messageBody
        );
    }
}
