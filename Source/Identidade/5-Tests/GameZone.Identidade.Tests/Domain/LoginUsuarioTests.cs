using Bogus;
using GameZone.Core.DomainObjects;
using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Tests.Api.Fixtures;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Identidade.Tests.Api.Domain
{
    [Collection(nameof(LoginUsuarioTestsFixtureCollection))]
    public class LoginUsuarioTests
    {
        private readonly Faker<LoginUsuarioDto> _faker;
        private readonly LoginUsuarioTestsFixture _loginUsuarioTestsFixture;

        public LoginUsuarioTests(LoginUsuarioTestsFixture loginUsuarioTestsFixture)
        {
            _faker = new Faker<LoginUsuarioDto>();
            _loginUsuarioTestsFixture = loginUsuarioTestsFixture;
        }

        [Fact(DisplayName = "Validando se a classe esta correta")]
        [Trait("Categoria", "Validando Login Usuário")]
        public void ValidateDomain_Should_LoginUser_Return_Success()
        {
            // Arrange
            var news = _loginUsuarioTestsFixture.LoginUserSuccess();
        }

        [Fact(DisplayName = "Validando se email do usuário para cadastro esta vazio")]
        [Trait("Categoria", "Validando Login Usuário")]
        public void ValidateDomain_ShouldThrowException_WhenEmailIsEmpty()
        {
            // Arrange
            var LoginUsuario = _loginUsuarioTestsFixture.LoginUserWithEmailEmpty();

            // Act
            var validationContext = new ValidationContext(LoginUsuario);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            bool isValid = Validator.TryValidateObject(LoginUsuario, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Equal("O Campo 'Email' é Obrigatório.", validationResults[0].ErrorMessage);
        }
    
        [Fact(DisplayName = "Validando se email do usuário para cadastro ultrapasssou o limite de caracteres")]
        [Trait("Categoria", "Validando Login Usuário")]
        public void ValidateDomain_ShouldThrowException_WhenEmailMaxLength()
        {
            // Arrange
            var LoginUsuario = _loginUsuarioTestsFixture.LoginUserWithEmailMaxLength();

            // Act
            var validationErrors = GetValidationErrors.GetListValidationErrors(LoginUsuario);

            // Assert
            Assert.NotEmpty(validationErrors);
            Assert.Contains("256 é o tamanho máximo para o campo 'Email'", validationErrors);
        }

        [Fact(DisplayName = "Validando se email do usuário para cadastro não é valido")]
        [Trait("Categoria", "Validando Login Usuário")]
        public void ValidateDomain_ShouldThrowException_WhenEmailIsNotValid()
        {
            // Arrange
            var LoginUsuario = _loginUsuarioTestsFixture.LoginUserWithEmailIsNotValid();

            // Act
            var validationErrors = GetValidationErrors.GetListValidationErrors(LoginUsuario);

            // Assert
            Assert.NotEmpty(validationErrors);
            Assert.Contains("O campo 'Email' não contém um endereço de email válido.", validationErrors);
        }

        [Fact(DisplayName = "Validando se Password do usuário para cadastro esta vazio")]
        [Trait("Categoria", "Validando Login Usuário")]
        public void ValidateDomain_ShouldThrowException_WhenPasswordIsEmpty()
        {
            // Arrange
            var LoginUsuario = _loginUsuarioTestsFixture.LoginUserWithPasswordIsEmpty();

            // Act
            var validationContext = new ValidationContext(LoginUsuario);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            bool isValid = Validator.TryValidateObject(LoginUsuario, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Equal("O Campo 'Password' é Obrigatório.", validationResults[0].ErrorMessage);
        }

        [Fact(DisplayName = "Validando se Password do usuário para cadastro ultrapasssou o limite de caracteres")]
        [Trait("Categoria", "Validando Login Usuário")]
        public void ValidateDomain_ShouldThrowException_WhenPasswordMaxLength()
        {
            // Arrange
            var LoginUsuario = _loginUsuarioTestsFixture.LoginUserWithPasswordMaxLength();

            // Act
            var validationErrors = GetValidationErrors.GetListValidationErrors(LoginUsuario);

            // Assert
            Assert.NotEmpty(validationErrors);
            Assert.Contains("20 é o tamanho máximo para o campo 'Password'", validationErrors);
        }
    }
}
