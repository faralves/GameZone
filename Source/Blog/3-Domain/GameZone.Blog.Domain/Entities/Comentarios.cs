using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZone.Blog.Domain.Entities
{
    [Table("Comentario")]
    public class Comentarios
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Noticia")]
        public int NoticiaId { get; set; }
        public Noticia Noticia { get; set; }

        [ForeignKey("AspNetUsers")]
        public string AspNetUsersId { get; set; }
        public Usuario AspNetUsers { get; set; }

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
