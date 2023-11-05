using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using GameZone.Core.DomainObjects;
using GameZone.Identidade.Application.DTOs;
using System.Text;

namespace GameZone.Identidade.Tests.Api.Fixtures
{
    public class CreateUsuarioTestsFixture
    {
        private readonly Faker _faker;

        public CreateUsuarioTestsFixture()
        {
            _faker = new Faker();
        }

        public CreateUsuarioDto CreateUserPF()
        {
            var name = _faker.Person.FullName;
            var cpfCnpj = _faker.Person.Cpf(true);
            var dataNascimento = _faker.Person.DateOfBirth;
            var email = _faker.Person.Email;
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }
        public CreateUsuarioDto CreateUserPJ()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(false);
            var dataNascimento = _faker.Date.Past(10);
            var email = _faker.Internet.Email();
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }


        public CreateUsuarioDto CreateUserWithNameEmpty()
        {
            var name = string.Empty;
            //var name = _faker.Person.FullName;
            var cpfCnpj = _faker.Person.Cpf(false);
            var dataNascimento = _faker.Person.DateOfBirth;
            var email = _faker.Person.Email;
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }
        
        public CreateUsuarioDto CreateUserWithNameMaxLength()
        {
            var name = _faker.Random.String2(257);
            var cpfCnpj = _faker.Person.Cpf(false);
            var dataNascimento = _faker.Person.DateOfBirth;
            var email = _faker.Person.Email;
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }

        public CreateUsuarioDto CreateUserWithCpfCnpjEmpty()
        {
            var cpfCnpj = string.Empty;
            var name = _faker.Person.FullName;
            var dataNascimento = _faker.Person.DateOfBirth;
            var email = _faker.Person.Email;
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }

        public CreateUsuarioDto CreateUserWithCpfCnpjMaxLength()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(true);
            var dataNascimento = _faker.Date.Past();
            var email = _faker.Internet.Email();
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }

        public CreateUsuarioDto CreateUserWithDataNascimentoMin()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(false);
            var dataNascimento = DateTime.MinValue;
            var email = _faker.Internet.Email();
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }

        public CreateUsuarioDto CreateUserWithDataNascimentoMax()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(false);
            var dataNascimento = DateTime.MaxValue;
            var email = _faker.Internet.Email();
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }

        public CreateUsuarioDto CreateUserWithEmailEmpty()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(false);
            var dataNascimento = _faker.Date.Past();
            var email = string.Empty;
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }

        public CreateUsuarioDto CreateUserWithEmailMaxLength()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(false);
            var dataNascimento = _faker.Date.Past();
            var email = _faker.Random.String2(257, "asfdasga@asfdgbasgba.com"); 
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }

        public CreateUsuarioDto CreateUserWithEmailIsNotValid()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(false);
            var dataNascimento = _faker.Date.Past();
            var email = _faker.Random.String2(256); 
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }


        public CreateUsuarioDto CreateUserWithPasswordIsEmpty()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(false);
            var dataNascimento = _faker.Date.Past();
            var email = _faker.Internet.Email();
            var password = string.Empty;
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }

        public CreateUsuarioDto CreateUserWithPasswordMaxLength()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(false);
            var dataNascimento = _faker.Date.Past();
            var email = _faker.Internet.Email();
            var password = _faker.Random.String2(21);
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }

        public CreateUsuarioDto CreateUserWithRePasswordIsEmpty()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(false);
            var dataNascimento = _faker.Date.Past();
            var email = _faker.Internet.Email();
            var password = _faker.Internet.Password();
            var rePassword = string.Empty;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }


        public CreateUsuarioDto CreateUserWithPasswordNoMatch()
        {
            var name = _faker.Person.FullName;
            var cpfCnpj = _faker.Person.Cpf(false);
            var dataNascimento = _faker.Person.DateOfBirth;
            var email = _faker.Person.Email;
            var password = _faker.Internet.Password();
            var rePassword = _faker.Lorem.Word();
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.Guid().ToString();

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }

        public CreateUsuarioDto CreateUserWithIdUsuarioInclusaoMaxLength()
        {
            var name = _faker.Company.CompanyName();
            var cpfCnpj = _faker.Company.Cnpj(false);
            var dataNascimento = _faker.Date.Past();
            var email = _faker.Internet.Email();
            var password = _faker.Internet.Password();
            var rePassword = password;
            var isAdministrator = _faker.Random.Bool();
            var idUsuarioInclusao = _faker.Random.String2(451);

            return new CreateUsuarioDto(name, cpfCnpj, dataNascimento, email, password, rePassword, isAdministrator, idUsuarioInclusao);
        }
    }
}
