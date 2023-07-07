using GameZone.Api.Controllers.Base;
using GameZone.Blog.Application.DTOs;
using GameZone.Blog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoticiasController : ControllerBase, IGenericControllerActions<CreateNoticiaDTO, int, NoticiaDTO>
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
            return Ok(noticias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var noticia = await _noticiaApplication.GetById(id);
            if (noticia == null)
            {
                return NotFound();
            }
            return Ok(noticia);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNoticiaDTO createNoticia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var noticia = await _noticiaApplication.Create(createNoticia);
            return CreatedAtAction(nameof(GetAll), new { id = noticia.Id }, noticia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NoticiaDTO noticia)
        {
            if (id != noticia.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var noticiaDb = await _noticiaApplication.GetById(id);
            if (noticiaDb == null)
            {
                return NotFound();
            }

            try
            {
                var noticiaAtualizada = await _noticiaApplication.Update(noticia);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Lidar com erros de atualização
                return StatusCode(500, "Ocorreu um erro enquanto atualizava a notícia.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var noticia = await _noticiaApplication.GetById(id);
            if (noticia == null)
            {
                return NotFound();
            }

            await _noticiaApplication.Delete(id);
            return NoContent();
        }
    }
}
