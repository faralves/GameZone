using System.ComponentModel.DataAnnotations;

namespace GameZone.News.WebApp.Models.DTO.Response
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public int NoticiaId { get; set; }

        public Guid AspNetUsersId { get; set; }

        public string NomeUsuario { get; set; }

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
