using GameZone.Core.DomainObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameZone.Identidade.Application.DTOs
{
    public class CreateUsuarioDto
    {
        [Required(ErrorMessage = "O Campo '{0}' e Obrigatorio.")]
        [MaxLength(256, ErrorMessage = "{1} e o tamanho maximo para o campo '{0}'")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' e Obrigatorio.")]
        [MaxLength(15,ErrorMessage ="O tamanho maximo para o campo '{0}' e de {1}"), MinLength(11, ErrorMessage = "O tamanho minimo para o campo '{0}' e de {1}")]
        public string CpfCnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} e obrigatorio.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31", ErrorMessage = "A data de nascimento deve estar entre 01/01/1900 e 31/12/9999.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O Campo '{0}' e Obrigatorio.")]
        [EmailAddress(ErrorMessage = "O campo '{0}' nao contem um endereço de email valido.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "O Campo '{0}' nao contem um endereço de email valido.")]
        [MaxLength(256, ErrorMessage = "{1} e o tamanho maximo para o campo '{0}'")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' e Obrigatorio.")]
        [MaxLength(20, ErrorMessage = "{1} e o tamanho maximo para o campo '{0}'")]
        [DataType(DataType.Password)]
        public string Password { get; set;} = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' e Obrigatorio.")]
        [Compare("Password", ErrorMessage = "As senhas nao conferem")]
        public string RePassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Campo '{0}' e Obrigatorio.")]
        public bool IsAdministrator { get; set; }

        [MaxLength(450, ErrorMessage = "{1} e o tamanho maximo para o campo '{0}'")]
        public string? IdUsuarioInclusao { get; set; }

        [JsonIgnore]
        public string UserName
        {
            get { return Email; }
        }

        public CreateUsuarioDto()
        {
                
        }

        public CreateUsuarioDto(string name, string cpfCnpj, DateTime dataNascimento, string email, string password, string rePassword, bool isAdministrator, string? idUsuarioInclusao)
        {
            Name = name;
            CpfCnpj = cpfCnpj;
            DataNascimento = dataNascimento;
            Email = email;
            Password = password;
            RePassword = rePassword;
            IsAdministrator = isAdministrator;
            IdUsuarioInclusao = idUsuarioInclusao;

            ValidateDTO();
        }

        private void ValidateDTO()
        {
            ValidarErrosTestes.AssertArgumentNotEmpty(Name, "O Campo 'Name' e Obrigatorio.");
            ValidarErrosTestes.AssertArgumentLength(Name, 256, "256 e o tamanho maximo para o campo 'Name'");

            ValidarErrosTestes.AssertArgumentNotEmpty(CpfCnpj, "O Campo 'CpfCnpj' e Obrigatorio.");
            ValidarErrosTestes.AssertArgumentLength(CpfCnpj, 11, 15, "O campo 'CpfCnpj' precisa estar entre 11 e 15 caracteres");
            
            ValidarErrosTestes.AssertArgumentNotNull(DataNascimento, "O Campo 'DataNascimento' e Obrigatorio.");
            ValidarErrosTestes.AssertArgumentDateTimeMinValue(DataNascimento, "A 'DataNascimento' nao pode ser a menor data do .Net!");
            ValidarErrosTestes.AssertArgumentDateTimeMaxValue(DataNascimento, "A 'DataNascimento' nao pode ser a maior data do .Net!");

            ValidarErrosTestes.AssertArgumentNotEmpty(Email, "O Campo 'Email' e Obrigatorio.");
            ValidarErrosTestes.AssertArgumentLength(Email, 256, "256 e o tamanho maximo para o campo 'Email'");
            ValidarErrosTestes.AssertArgumentValidEmail(Email, "O Campo 'Email' nao e valido.");

            ValidarErrosTestes.AssertArgumentNotEmpty(Password, "O Campo 'Password' e Obrigatorio.");
            ValidarErrosTestes.AssertArgumentLength(Password, 20, "20 e o tamanho maximo para o campo 'Password'");

            ValidarErrosTestes.AssertArgumentNotEmpty(RePassword, "O Campo 'RePassword' e Obrigatorio.");
            ValidarErrosTestes.AssertArgumentPasswordsMatch(Password, RePassword, "As senhas nao conferem!");

            ValidarErrosTestes.AssertArgumentLength(IdUsuarioInclusao, 450, "450 e o tamanho maximo para o campo 'IdUsuarioInclusao'");
        }
    }
}
