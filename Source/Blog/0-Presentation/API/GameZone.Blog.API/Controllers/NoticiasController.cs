using GameZone.Api.Controllers.Base;
using GameZone.Blog.API.Authorization;
using GameZone.Blog.Application.DTOs.Request;
using GameZone.Blog.Application.Interfaces;
using GameZone.Blog.Domain.Entities;
using GameZone.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Blog.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class NoticiasController : ControllerBase, IGenericControllerActions<CreateNoticiaDTO, int, UpdateNoticiaDTO>
    {
        private readonly INoticiaApplication _noticiaApplication;

        public NoticiasController(INoticiaApplication noticiaApplication)
        {
            _noticiaApplication = noticiaApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var noticias = await _noticiaApplication.GetAll();
            if (!noticias.Any())
                return NotFound();

            return Ok(noticias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var noticia = await _noticiaApplication.GetById(id);
            if (noticia == null)
                return NotFound();

            return Ok(noticia);
        }

        [Authorize]
        [InserirNoticiaAuthorization]
        [HttpPost]
        public async Task<IActionResult> Create(CreateNoticiaDTO createNoticia)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o usuário está autenticado
            if (User.Identity.IsAuthenticated)
            {
                //var claims = User.Claims.ToList();

                var idUsuarioClaim = User.GetUserId();

                var noticia = await _noticiaApplication.Create(createNoticia, idUsuarioClaim);
                return CreatedAtAction(nameof(Create), new { id = noticia.Id }, noticia);
            }
            else
            {
                return Unauthorized();
            }

        }

        [Authorize]
        [EditarNoticiaAuthorization]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateNoticiaDTO noticia)
        {
            if (id != noticia.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var noticiaDb = await _noticiaApplication.GetById(id);
            if (noticiaDb == null)
                return NotFound();

            try
            {
                var idUsuario = string.Empty;
                if (User.Identity.IsAuthenticated)
                    idUsuario = User.GetUserId();
                else
                    return Unauthorized();

                var noticiaAtualizada = await _noticiaApplication.Update(noticia, idUsuario);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Lidar com erros de atualização
                return StatusCode(500, "Ocorreu um erro enquanto atualizava a notícia.");
            }
        }

        [Authorize]
        [ExcluirNoticiaAuthorization]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var noticia = await _noticiaApplication.GetById(id);
            if (noticia == null)
                return NotFound();

            await _noticiaApplication.Delete(id);
            return NoContent();
        }
    }
}
