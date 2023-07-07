using System;

namespace GameZone.Core.Messages.Integration
{
    public class UsuarioRegistradoIntegrationEvent : IntegrationEvent
    {
        public Guid IdAspNetUsers { get; private set; }
        public string Nome { get; private set; }
        public string CpfCnpj { get; private set; }
        public string Email { get; private set; }
        public string ChaveAcesso { get; private set; }
        public bool Pericia { get; private set; }
        public string RazaoSocial { get; private set; }
        public string Observacoes { get; private set; }
        public string RgIe { get; private set; }
        public string Endereco { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public int IdCidade { get; private set; }
        public string Cep { get; private set; }
        public string Numero { get; private set; }
        public string DddTelefone { get; private set; }
        public string Telefone { get; private set; }
        public string DddFax { get; private set; }
        public string Fax { get; private set; }
        public int LimiteDiarioConsulta { get; private set; }
        public int LimiteMensalConsulta { get; private set; }

        public UsuarioRegistradoIntegrationEvent(Guid idAspNetUsers, string nome, string cpfCnpj, string email, string chaveAcesso, bool pericia, string razaoSocial, string observacoes, string rgIe, string endereco, string complemento, string bairro, int idCidade, 
                                                 string cep, string numero, string dddTelefone, string telefone, string dddFax, string fax, int limiteDiarioConsulta, int limiteMensalConsulta)
        {
            IdAspNetUsers = idAspNetUsers;
            Nome = nome;
            Email = email;
            CpfCnpj = cpfCnpj;
            ChaveAcesso = chaveAcesso;
            Pericia = pericia;
            RazaoSocial = razaoSocial;
            Observacoes = observacoes;
            RgIe = rgIe;
            Endereco = endereco;
            Complemento = complemento;
            Bairro = bairro;
            IdCidade = idCidade;
            Cep = cep;
            Numero = numero;
            DddTelefone = dddTelefone;
            Telefone = telefone;
            DddFax = dddFax;
            Fax = fax;
            LimiteDiarioConsulta = limiteDiarioConsulta;
            LimiteMensalConsulta = limiteMensalConsulta;
        }
    }
}