using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameZone.News.WebApp.Models.DTO.Request
{
    public class UpdateCommentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(800)]
        public string Opiniao { get; set; } = string.Empty;
    }
}
