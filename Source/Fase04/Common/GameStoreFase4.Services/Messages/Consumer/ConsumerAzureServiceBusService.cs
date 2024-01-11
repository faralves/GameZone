using GameStoreFase4.Domain.Entities;

namespace GameStoreFase4.Services.Messages.Consumer;
public class ConsumerAzureServiceBusService : IConsumerAzureServiceBusService
{
    public ObjectMessageProcessedInfo Consume(bool consumeDlq = false)
    {
        throw new NotImplementedException();
    }
}
