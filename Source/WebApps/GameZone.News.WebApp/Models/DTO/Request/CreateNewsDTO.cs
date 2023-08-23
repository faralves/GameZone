using GameZone.WebAPI.Core;
using System.ComponentModel.DataAnnotations;

namespace GameZone.News.WebApp.Models.DTO.Request
{
    public class CreateNewsDTO
    {
        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(int.MaxValue, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Chapeu { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31", ErrorMessage = "A data de nascimento deve estar entre 01/01/1900 e 31/12/9999.")]
        public DateTime DataPublicacao { get; set; } = DateTime.Now;

        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Autor { get; set; } = string.Empty;

        [Display(Name = "Arquivo")]
        public IFormFile? Arquivo { get; set; }

        public string Database64Content { get; set; } = string.Empty;

        public byte[] DataStream { get; set; } = new byte[0];

        private string urlImagem = string.Empty;
        [MaxLength(300, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string UrlImagem
        {
            get { return urlImagem; }
            set
            {
                urlImagem = value;
                if(!string.IsNullOrEmpty(urlImagem) && !urlImagem.Contains("http"))
                {
                    DataStream = Service.GetDataStream(urlImagem);
                    Database64Content = DataStream != null ? Service.GetDatabase64(DataStream) : !string.IsNullOrEmpty(Database64Content) ? Database64Content : string.Empty;
                }
            }
        }
    }
}
