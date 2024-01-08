using GameStoreFase4.Domain.Entities;
using GameStoreFase4.Services.Messages.Producer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameStoreFase4.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JogoController : ControllerBase
{
    private readonly IProducerRabbitMqService _producerRabbitMqService;
    private bool _enableDlq = false;

    public JogoController(IProducerRabbitMqService producerRabbitMqService)
    {
        _producerRabbitMqService = producerRabbitMqService;
    }

    [HttpGet]
    public ActionResult Get()
    {
        return Ok("Api de Jogos");
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Jogo jogo)
    {
        var message = JsonConvert.SerializeObject(jogo);
        await _producerRabbitMqService.Publish(message, typeof(Jogo), _enableDlq);
        return Ok($"Processamento efetuado com sucesso para o jogo: {jogo.Nome}");
    }

    [HttpPut]
    public async Task<ActionResult> Put([FromBody] Jogo jogo)
    {
        var message = JsonConvert.SerializeObject(jogo);
        await _producerRabbitMqService.Publish(message, typeof(Jogo), _enableDlq);
        return Ok($"Processamento efetuado com sucesso para o jogo: {jogo.Nome}");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var message = JsonConvert.SerializeObject(new Jogo { Id = id });
        await _producerRabbitMqService.Publish(message, typeof(Jogo), _enableDlq);
        return Ok($"Processamento efetuado com sucesso");
    }

}
