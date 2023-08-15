using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameZone.Identidade.Domain.Entities
{
    public class LoginUsuario
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set;} = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}
