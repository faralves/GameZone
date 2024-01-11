using GameStoreFase4.Domain.Parameters;

namespace GameStoreFase4.Services.Parameters;
public class ParameterService : IParameterService
{
    public void EnableAzureServiceBus()
    {
        GlobalParameters.ENABLE_AZURE_SERVICE_BUS_MQ_MESSAGE_SERVICE = true;
        GlobalParameters.ENABLE_RABBIT_MQ_MESSAGE_SERVICE = false;
        GlobalParameters.ENABLE_MASS_TRANSIT_MESSAGE_SERVICE = false;
    }

    public void EnableMassTransit()
    {
        GlobalParameters.ENABLE_MASS_TRANSIT_MESSAGE_SERVICE = true;
        GlobalParameters.ENABLE_RABBIT_MQ_MESSAGE_SERVICE = false;
        GlobalParameters.ENABLE_AZURE_SERVICE_BUS_MQ_MESSAGE_SERVICE = false;
    }

    public void EnableRabbitMq()
    {
        GlobalParameters.ENABLE_RABBIT_MQ_MESSAGE_SERVICE = true;
        GlobalParameters.ENABLE_MASS_TRANSIT_MESSAGE_SERVICE = false;
        GlobalParameters.ENABLE_AZURE_SERVICE_BUS_MQ_MESSAGE_SERVICE = false;
    }
}
