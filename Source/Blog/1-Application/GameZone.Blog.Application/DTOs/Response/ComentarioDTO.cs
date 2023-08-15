using System.ComponentModel.DataAnnotations;

namespace GameZone.Blog.Application.DTOs.Response
{
    public class ComentarioDTO
    {
        public int Id { get; set; }

        public int NoticiaId { get; set; }

        public Guid AspNetUsersId { get; set; }

        public string Autor { get; set; }

        [MaxLength(800)]
        public string Comentario { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataCriacao { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataAtualizacao { get; set; }
    }
}
