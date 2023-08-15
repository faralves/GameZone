using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameZone.News.WebApp.Models.DTO.Request
{
    public class CreateCommentDTO
    {
        public int NoticiaId { get; set; } = 0;

        [MaxLength(800)]
        public string Comentario { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
