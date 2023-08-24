using GameZone.Api.Controllers.Base;
using GameZone.Blog.API.Authorization;
using GameZone.Blog.Application;
using GameZone.Blog.Application.DTOs.Request;
using GameZone.Blog.Application.Interfaces;
using GameZone.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace GameZone.Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentarioController : ControllerBase, IGenericControllerActions<CreateComentarioDTO, int, UpdateComentarioDTO>
    {
        private readonly IComentarioApplication _comentarioApplication;

        public ComentarioController(IComentarioApplication comentarioApplication)
        {
            _comentarioApplication = comentarioApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comentario = await _comentarioApplication.GetAll();
            if (!comentario.Any())
                return NotFound();

            return Ok(comentario);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comentario = await _comentarioApplication.GetById(id);
            if (comentario == null)
                return NotFound();

            return Ok(comentario);
        }

        [HttpGet("GetByNoticiaId/{id}")]
        public async Task<IActionResult> GetByNoticiaId(int id)
        {
            var comentario = await _comentarioApplication.GetByNoticiaId(id);
            if (!comentario.Any())
                return NotFound();

            return Ok(comentario);
        }

        [Authorize]
        [InserirComentarioAuthorization]
        [HttpPost]
        public async Task<IActionResult> Create(CreateComentarioDTO createComentario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se o usuário está autenticado
            if (User.Identity.IsAuthenticated)
            {
                var idUsuarioClaim = User.GetUserId();

                var comentario = await _comentarioApplication.Create(createComentario, idUsuarioClaim);
                return CreatedAtAction(nameof(Create), new { id = comentario.Id }, comentario);
            }
            else
            {
                return Unauthorized();
            }

        }

        [Authorize]
        [EditarComentarioAuthorization]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateComentarioDTO comentario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comentarioDb = await _comentarioApplication.GetById(id);
            if (comentarioDb == null)
                return NotFound();

            var idUsuarioClaim = new Guid(User.GetUserId());
            if (idUsuarioClaim != comentarioDb.AspNetUsersId)
                return BadRequest("O usuário que está atualizando não corresponde ao Autor.");

            try
            {
                comentario.AspNetUsersId = idUsuarioClaim;
                comentario.DataCriacao = comentarioDb.DataCriacao;

                comentario.Id = comentarioDb.Id;

                var comentarioAtualizado = await _comentarioApplication.Update(comentario, comentario.AspNetUsersId.ToString());
                return NoContent();
            }
            catch (Exception ex)
            {
                // Lidar com erros de atualização
                return StatusCode(500, "Ocorreu um erro enquanto atualizava o comentário.");
            }
        }

        [Authorize]
        [ExcluirComentarioAuthorization]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comentario = await _comentarioApplication.GetById(id);
            if (comentario == null)
                return NotFound();

            await _comentarioApplication.Delete(id);
            return NoContent();
        }
    }
}
