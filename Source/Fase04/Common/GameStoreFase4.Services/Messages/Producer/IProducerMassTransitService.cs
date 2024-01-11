namespace GameStoreFase4.Services.Messages.Producer;
public interface IProducerMassTransitService
{
    Task Publish(string message, Type objectType, bool publishDlq = false);
}
