using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameZone.News.WebApp.Models.DTO.Response
{
    public class NewsDto
    {
        public int Id { get; set; }

        public Guid AspNetUsersId { get; set; }

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
        public ICollection<CommentDTO> Comentarios { get; set; }
        public CreateCommentDTO CreateComentario { get; set; }
    }
}
