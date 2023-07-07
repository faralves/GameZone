using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameZone.Identidade.Application.DTOs
{
    public class CreateUsuarioDto
    {
        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(256, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(15,ErrorMessage ="O tamanho máximo para o campo '{0}' é de {1}"), MinLength(11, ErrorMessage = "O tamanho mínimo para o campo '{0}' é de {1}")]
        public string CpfCnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01/01/1900", "31/12/9999", ErrorMessage = "A data de nascimento deve estar entre 01/01/1900 e 31/12/9999.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo '{0}' não contém um endereço de email válido.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "O Campo '{0}' não contém um endereço de email válido.")]
        [MaxLength(256, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [MaxLength(20, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        [DataType(DataType.Password)]
        public string Password { get; set;} = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        [Compare("Password", ErrorMessage = "As senhas não conferem")]
        public string RePassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' é Obrigatório.")]
        public bool IsAdministrator { get; set; }

        [MaxLength(450, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string? IdUsuarioInclusao { get; set; }

        [JsonIgnore]
        public string UserName
        {
            get { return Email; }
        }
    }
}
