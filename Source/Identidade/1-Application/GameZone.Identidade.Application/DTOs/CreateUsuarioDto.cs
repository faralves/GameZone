using GameZone.Core.DomainObjects;
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
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31", ErrorMessage = "A data de nascimento deve estar entre 01/01/1900 e 31/12/9999.")]
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
            ValidarErrosTestes.AssertArgumentNotEmpty(Name, "O Campo 'Name' é Obrigatório.");
            ValidarErrosTestes.AssertArgumentLength(Name, 256, "256 é o tamanho máximo para o campo 'Name'");

            ValidarErrosTestes.AssertArgumentNotEmpty(CpfCnpj, "O Campo 'CpfCnpj' é Obrigatório.");
            ValidarErrosTestes.AssertArgumentLength(CpfCnpj, 11, 15, "O campo 'CpfCnpj' precisa estar entre 11 e 15 caracteres");
            
            ValidarErrosTestes.AssertArgumentNotNull(DataNascimento, "O Campo 'DataNascimento' é Obrigatório.");
            ValidarErrosTestes.AssertArgumentDateTimeMinValue(DataNascimento, "A 'DataNascimento' não pode ser a menor data do .Net!");
            ValidarErrosTestes.AssertArgumentDateTimeMaxValue(DataNascimento, "A 'DataNascimento' não pode ser a maior data do .Net!");

            ValidarErrosTestes.AssertArgumentNotEmpty(Email, "O Campo 'Email' é Obrigatório.");
            ValidarErrosTestes.AssertArgumentLength(Email, 256, "256 é o tamanho máximo para o campo 'Email'");
            ValidarErrosTestes.AssertArgumentValidEmail(Email, "O Campo 'Email' não é válido.");

            ValidarErrosTestes.AssertArgumentNotEmpty(Password, "O Campo 'Password' é Obrigatório.");
            ValidarErrosTestes.AssertArgumentLength(Password, 20, "20 é o tamanho máximo para o campo 'Password'");

            ValidarErrosTestes.AssertArgumentNotEmpty(RePassword, "O Campo 'RePassword' é Obrigatório.");
            ValidarErrosTestes.AssertArgumentPasswordsMatch(Password, RePassword, "As senhas não conferem!");

            ValidarErrosTestes.AssertArgumentLength(IdUsuarioInclusao, 450, "450 é o tamanho máximo para o campo 'IdUsuarioInclusao'");
        }
    }
}
