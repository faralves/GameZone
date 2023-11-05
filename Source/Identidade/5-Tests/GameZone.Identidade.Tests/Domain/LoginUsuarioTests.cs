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
        [Trait("Categoria", "Validando Login Usuario")]
        public void ValidateDomain_Should_LoginUser_Return_Success()
        {
            // Arrange
            var news = _loginUsuarioTestsFixture.LoginUserSuccess();
        }

        [Fact(DisplayName = "Validando se email do usuario para cadastro esta vazio")]
        [Trait("Categoria", "Validando Login Usuario")]
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
            Assert.Equal("O Campo 'Email' e Obrigatorio.", validationResults[0].ErrorMessage);
        }
    
        [Fact(DisplayName = "Validando se email do usuario para cadastro ultrapasssou o limite de caracteres")]
        [Trait("Categoria", "Validando Login Usuario")]
        public void ValidateDomain_ShouldThrowException_WhenEmailMaxLength()
        {
            // Arrange
            var LoginUsuario = _loginUsuarioTestsFixture.LoginUserWithEmailMaxLength();

            // Act
            var validationErrors = GetValidationErrors.GetListValidationErrors(LoginUsuario);

            // Assert
            Assert.NotEmpty(validationErrors);
            Assert.Contains("256 e o tamanho maximo para o campo 'Email'", validationErrors);
        }

        [Fact(DisplayName = "Validando se email do usuario para cadastro nao e valido")]
        [Trait("Categoria", "Validando Login Usuario")]
        public void ValidateDomain_ShouldThrowException_WhenEmailIsNotValid()
        {
            // Arrange
            var LoginUsuario = _loginUsuarioTestsFixture.LoginUserWithEmailIsNotValid();

            // Act
            var validationErrors = GetValidationErrors.GetListValidationErrors(LoginUsuario);

            // Assert
            Assert.NotEmpty(validationErrors);
            Assert.Contains("O campo 'Email' nao contem um endereço de email valido.", validationErrors);
        }

        [Fact(DisplayName = "Validando se Password do usuario para cadastro esta vazio")]
        [Trait("Categoria", "Validando Login Usuario")]
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
            Assert.Equal("O Campo 'Password' e Obrigatorio.", validationResults[0].ErrorMessage);
        }

        [Fact(DisplayName = "Validando se Password do usuario para cadastro ultrapasssou o limite de caracteres")]
        [Trait("Categoria", "Validando Login Usuario")]
        public void ValidateDomain_ShouldThrowException_WhenPasswordMaxLength()
        {
            // Arrange
            var LoginUsuario = _loginUsuarioTestsFixture.LoginUserWithPasswordMaxLength();

            // Act
            var validationErrors = GetValidationErrors.GetListValidationErrors(LoginUsuario);

            // Assert
            Assert.NotEmpty(validationErrors);
            Assert.Contains("20 e o tamanho maximo para o campo 'Password'", validationErrors);
        }
    }
}
