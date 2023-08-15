using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZone.Blog.Domain.Entities
{
    [Table("Noticia")]
    public class Noticia
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AspNetUsers")]
        public string AspNetUsersId { get; set; }
        public Usuario AspNetUsers { get; set; }

        [MaxLength(255)]
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        
        [MaxLength(255)] 
        public string Chapeu { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataPublicacao { get; set; }
        
        [MaxLength(255)] 
        public string Autor { get; set; } = string.Empty;

        [MaxLength(300)] 
        public string? UrlImagem { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataAtualizacao { get; set; }

        // Propriedade de navegação para os Comentarios relacionados à Noticia
        public ICollection<Comentarios> Comentarios { get; set; }
    }
}
