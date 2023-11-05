using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameZone.Identidade.Application.DTOs
{
    public class LoginUsuarioDto
    {
        [Required(ErrorMessage = "O Campo '{0}' e Obrigatorio.")]
        [EmailAddress(ErrorMessage = "O campo '{0}' nao contem um endereço de email valido.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "O Campo '{0}' nao contem um endereço de email valido.")]
        [MaxLength(256, ErrorMessage = "{1} e o tamanho maximo para o campo '{0}'")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' e Obrigatorio.")]
        [MaxLength(20, ErrorMessage = "{1} e o tamanho maximo para o campo '{0}'")]
        [DataType(DataType.Password)]
        public string Password { get; set;} = string.Empty;

        [JsonIgnore]
        public string UserName
        {
            get { return Email; }
        }
    }
}
