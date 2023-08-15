using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameZone.Blog.Application.DTOs.Request
{
    public class UpdateComentarioDTO
    {
        [JsonIgnore]
        public int Id { get; set; } 

        public int NoticiaId { get; set; }

        [JsonIgnore]
        public Guid AspNetUsersId { get; set; } = new Guid();

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(800)]
        public string Comentario { get; set; } = string.Empty;

        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataCriacao { get; set; } = DateTime.MinValue;

        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataAtualizacao { get; set; } = DateTime.Now;
    }
}
