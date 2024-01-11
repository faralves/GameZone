namespace GameStoreFase4.Services.Messages.Producer;
public class ProducerMassTransitService : IProducerMassTransitService
{
    public async Task Publish(string message, Type objectType, bool publishDlq = false)
    {
        throw new NotImplementedException();
    }
}
