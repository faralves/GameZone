using GameZone.WebAPI.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameZone.Blog.Application.DTOs.Request
{
    public class UpdateNoticiaDTO
    {
        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(int.MaxValue, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Chapeu { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31", ErrorMessage = "A data de Publicacao deve estar entre 01/01/1900 e 31/12/9999.")]
        public DateTime DataPublicacao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31", ErrorMessage = "A data de Atualização deve estar entre 01/01/1900 e 31/12/9999.")]
        public DateTime? DataAtualizacao { get; set; } 

        private string urlImagem;

        [MaxLength(300, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string UrlImagem
        {
            get { return urlImagem; }
            set{ urlImagem = value; }
        }

        private void AtualizarInfosImagem()
        {
            DataStream = Service.GetDataStream(UrlImagem);
            Database64Content = DataStream != null ? Service.GetDatabase64(DataStream) : !string.IsNullOrEmpty(Database64Content) ? Database64Content : string.Empty;
        }

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Autor { get; set; } = string.Empty;
        

        public string Database64Content { get; set; } = string.Empty;

        [JsonIgnore]
        public byte[] DataStream { get; set; } = new byte[0];

        [JsonIgnore]
        public string UrlBlobStorage { get; set; } = string.Empty;

    }
}
