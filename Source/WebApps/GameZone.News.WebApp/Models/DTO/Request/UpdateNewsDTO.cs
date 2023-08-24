using GameZone.WebAPI.Core;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GameZone.News.WebApp.Models.DTO.Request
{
    public class UpdateNewsDTO
    {
        public int Id { get; set; }

        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(int.MaxValue, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Descricao { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Chapeu { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime DataPublicacao { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataAtualizacao { get; set; } = DateTime.Now;

        private string urlImagem;

        [MaxLength(300, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string UrlImagem
        {
            get { return urlImagem; }
            set
            {
                urlImagem = value;
                DataStream = Service.GetDataStream(UrlImagem);
                Database64Content = DataStream != null ? Service.GetDatabase64(DataStream) : !string.IsNullOrEmpty(Database64Content) ? Database64Content : string.Empty;
            }
        }

        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Autor { get; set; } = string.Empty;

        public string Database64Content { get; set; } = string.Empty;

        [JsonIgnore]
        public Guid UsuarioId { get; set; }

        [JsonIgnore]
        public byte[] DataStream { get; set; } = new byte[0];

        [JsonIgnore]
        [Display(Name = "Arquivo")]
        public IFormFile? Arquivo { get; set; }
    }
}
