using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Identidade.Domain.Entidades
{
    public class Usuario : IdentityUser
    {
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(15)]
        public string CpfCnpj { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        public bool IsAdministrator { get; set; }

        [MaxLength(450, ErrorMessage = "{1} é o tamanho máximo para o campo '{0}'")]
        public string? IdUsuarioInclusao { get; set; }
    }
}
