using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Identidade.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AcessoController : ControllerBase
    {
        //[Authorize(Policy = "IdadeMinima")]
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("Acesso permitido!");

        }
    }
}
