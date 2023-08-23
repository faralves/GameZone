using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GameZone.WebAPI.Core;

namespace GameZone.News.WebApp.Models.DTO.Response
{
    public class NewsDto
    {
        public int Id { get; set; }

        public Guid AspNetUsersId { get; set; }

        [MaxLength(255)]
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;

        private string descricao;

        public string Descricao
        {
            get { return descricao; }
            set
            {
                descricao = value;
                if (!string.IsNullOrEmpty(descricao))
                {
                    Conteudo = descricao;
                }
            }
        }

        [MaxLength(255)]
        public string Chapeu { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataPublicacao { get; set; }

        [MaxLength(255)]
        public string Autor { get; set; } = string.Empty;

        private string? urlImagem = string.Empty;
        [MaxLength(300)]
        public string? UrlImagem
        {
            get { return urlImagem; }
            set
            {
                urlImagem = value;
                if (!string.IsNullOrEmpty(urlImagem) && !urlImagem.Contains("http"))
                {
                    DataStream = Service.GetDataStream(urlImagem);
                    Database64Content = DataStream != null ? Service.GetDatabase64(DataStream) : !string.IsNullOrEmpty(Database64Content) ? Database64Content : string.Empty;
                }
            }
        }
        public string Database64Content { get; set; }
        public byte[] DataStream { get; set; } = new byte[0];


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataAtualizacao { get; set; }

        // Propriedade de navegação para os Comentarios relacionados à Noticia
        public ICollection<CommentDTO> Comentarios { get; set; }
        public CreateCommentDTO CreateComentario { get; set; }
    }
}
