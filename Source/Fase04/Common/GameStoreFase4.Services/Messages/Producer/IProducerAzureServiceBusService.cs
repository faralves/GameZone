
namespace GameStoreFase4.Services.Messages.Producer;

public interface IProducerAzureServiceBusService
{
    Task Publish(string message, Type objectType, bool publishDlq = false);
}
