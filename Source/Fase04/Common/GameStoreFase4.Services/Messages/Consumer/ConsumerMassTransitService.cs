using GameStoreFase4.Domain.Entities;

namespace GameStoreFase4.Services.Messages.Consumer;
public class ConsumerMassTransitService : IConsumerMassTransitService
{
    public ObjectMessageProcessedInfo Consume(bool consumeDlq = false)
    {
        throw new NotImplementedException();
    }
}