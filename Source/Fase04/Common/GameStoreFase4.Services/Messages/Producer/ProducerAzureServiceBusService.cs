using Microsoft.Extensions.Configuration;

namespace GameStoreFase4.Services.Messages.Producer;
public class ProducerAzureServiceBusService : IProducerAzureServiceBusService
{
    private readonly IConfiguration _configuration;
    public ProducerAzureServiceBusService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Publish(string message, Type objectType, bool publishDlq = false)
    {
        throw new NotImplementedException();
    }
}