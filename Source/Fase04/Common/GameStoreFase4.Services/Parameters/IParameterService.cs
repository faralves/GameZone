namespace GameStoreFase4.Services.Parameters;
public interface IParameterService
{
    public void EnableRabbitMq();
    public void EnableMassTransit();
    public void EnableAzureServiceBus();
}
