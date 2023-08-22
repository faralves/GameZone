﻿using GameZone.WebAPI.Core;
using System.ComponentModel.DataAnnotations;

namespace GameZone.News.WebApp.Models.DTO.Request
{
    public class UpdateNewsDTO
    {
        public int Id { get; set; }

        public Guid UsuarioId { get; set; }

        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(int.MaxValue, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Descricao { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Chapeu { get; set; } = string.Empty;

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Range(typeof(DateTime), "1900-01-01", "9999-12-31", ErrorMessage = "A data de publicação deve estar entre 01/01/1900 e 31/12/9999.")]
        [DataType(DataType.Date)]
        public DateTime DataPublicacao { get; set; }

        [MaxLength(255, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Autor { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataAtualizacao { get; set; } = DateTime.Now;

        [Display(Name = "Arquivo")]
        public IFormFile? Arquivo { get; set; }

        public string Database64Content { get; set; }

        public byte[] DataStream { get; set; }

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
    }
}
