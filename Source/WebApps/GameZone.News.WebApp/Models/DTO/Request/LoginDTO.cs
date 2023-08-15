using Microsoft.AspNetCore.SignalR.Protocol;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GameZone.News.WebApp.Models.DTO.Request
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo '{0}' não contém um endereço de email válido.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "O Campo '{0}' não contém um endereço de email válido.")]
        [MaxLength(256, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(20, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [JsonIgnore]
        public string UserName
        {
            get { return Email; }
        }
    }
}
