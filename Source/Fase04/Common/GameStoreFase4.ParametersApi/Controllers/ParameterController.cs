using GameStoreFase4.Services.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreFase4.ParametersApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParameterController : ControllerBase
{
    private readonly IParameterService _parameterService;

    public ParameterController(IParameterService parameterService)
    {
        _parameterService = parameterService;
    }

    [HttpGet("/enable/rabbitmq")]
    public ActionResult EnableRabbitMq()
    {
        _parameterService.EnableRabbitMq();
        return Ok("Serviço do RabbitMq ativado para os consumers e producers");
    }

    [HttpGet("/enable/masstransit")]
    public ActionResult EnableMassTransit()
    {
        _parameterService.EnableMassTransit();
        return Ok("Serviço do MassTransit ativado para os consumers e producers");
    }

    [HttpGet("/enable/servicebus")]
    public ActionResult EnableAzureServiceBus()
    {
        _parameterService.EnableAzureServiceBus();
        return Ok("Serviço do Azure Service Bus ativado para os consumers e producers");
    }
}

