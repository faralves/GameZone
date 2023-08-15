﻿using GameZone.WebAPI.Core;
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01/01/1900", "31/12/9999", ErrorMessage = "A data de nascimento deve estar entre 01/01/1900 e 31/12/9999.")]
        public DateTime DataPublicacao { get; set; }

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

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Autor { get; set; } = string.Empty;

        [JsonIgnore]
        public byte[] DataStream { get; set; }

        [JsonIgnore]
        public string Database64Content { get; set; }

        [JsonIgnore]
        public string UrlBlobStorage { get; set; }

    }
}
