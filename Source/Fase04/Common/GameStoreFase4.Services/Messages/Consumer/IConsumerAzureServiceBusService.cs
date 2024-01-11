using GameStoreFase4.Domain.Entities;

namespace GameStoreFase4.Services.Messages.Consumer;
public interface IConsumerAzureServiceBusService
{
    ObjectMessageProcessedInfo Consume(bool consumeDlq = false);
}
