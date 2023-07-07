using System;

namespace GameZone.Core.Messages.Integration
{
    public class UsuarioRegistradoExistenteIntegrationEvent : IntegrationEvent
    {
        public Guid IdAspNetUsers { get; private set; }
        public string Nome { get; private set; }
        public string CpfCnpj { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; set; }

        public UsuarioRegistradoExistenteIntegrationEvent(Guid idAspNetUsers, string nome, string cpfCnpj, string email, string senha)
        {
            IdAspNetUsers = idAspNetUsers;
            Nome = nome;
            Email = email;
            CpfCnpj = cpfCnpj;
            Senha = senha;
        }
    }
}