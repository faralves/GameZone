using Bogus;
using GameZone.Core.DomainObjects;
using GameZone.Identidade.Tests.Api.Fixtures;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Identidade.Tests.Api.Domain
{
    [Collection(nameof(CreateUsuarioTestsFixtureCollection))]
    public class UsuarioTests
    {
        private readonly Faker _faker;
        private readonly CreateUsuarioTestsFixture _usuarioTestsFixture;

        public UsuarioTests(CreateUsuarioTestsFixture usuarioTestsFixture)
        {
            _faker = new Faker();
            _usuarioTestsFixture = usuarioTestsFixture;
        }

        [Fact(DisplayName = "Validando se a classe esta correta para Pessoa Fisica")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_Should_CreateUserPF_Return_Success()
        {
            // Arrange
            var usuario = _usuarioTestsFixture.CreateUserPF();
        }

        [Fact(DisplayName = "Validando se a classe esta correta para Pessoa Juridica")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_Should_CreateUserPJ_Return_Success()
        {
            // Arrange
            var usuario = _usuarioTestsFixture.CreateUserPJ();
        }


        [Fact(DisplayName = "Validando se o nome do usu�rio para cadastro est� vazio")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenNameIsEmpty()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithNameEmpty());

            //Assert
            Assert.Equal("O Campo 'Name' � Obrigat�rio.", result.Message);
        }

        [Fact(DisplayName = "Validando se o nome do usu�rio para cadastro ultrapasssou o limite de caracteres")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenNameMaxLength()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithNameMaxLength());

            //Assert
            Assert.Equal("256 � o tamanho m�ximo para o campo 'Name'", result.Message);
        }

        [Fact(DisplayName = "Validando se o CPF ou CNPJ do usu�rio para cadastro est� vazio")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenCpfCnpjIsEmpty()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithCpfCnpjEmpty());

            //Assert
            Assert.Equal("O Campo 'CpfCnpj' � Obrigat�rio.", result.Message);
        }

        [Fact(DisplayName = "Validando se o CPF ou CNPJ do usu�rio para cadastro ultrapasssou o limite de caracteres")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenCpfCnpjMaxLength()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithCpfCnpjMaxLength());

            //Assert
            Assert.Equal("O campo 'CpfCnpj' precisa estar entre 11 e 15 caracteres", result.Message);
        }

        [Fact(DisplayName = "Validando se a data de nascimento do usu�rio para cadastro � a data minima do .net")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenDataNascimentoMinValue()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithDataNascimentoMin());

            //Assert
            Assert.Equal("A 'DataNascimento' n�o pode ser a menor data do .Net!", result.Message);
        }

        [Fact(DisplayName = "Validando se a data de nascimento do usu�rio para cadastro � a data maxima do .net")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenDataNascimentoMaxValue()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithDataNascimentoMax());

            //Assert
            Assert.Equal("A 'DataNascimento' n�o pode ser a maior data do .Net!", result.Message);
        }

        [Fact(DisplayName = "Validando se email do usu�rio para cadastro esta vazio")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenEmailIsEmpty()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithEmailEmpty());

            //Assert
            Assert.Equal("O Campo 'Email' � Obrigat�rio.", result.Message);
        }

        [Fact(DisplayName = "Validando se email do usu�rio para cadastro ultrapasssou o limite de caracteres")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenEmailMaxLength()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithEmailMaxLength());

            //Assert
            Assert.Equal("256 � o tamanho m�ximo para o campo 'Email'", result.Message);
        }

        [Fact(DisplayName = "Validando se email do usu�rio para cadastro n�o � valido")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenEmailIsNotValid()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithEmailIsNotValid());

            //Assert
            Assert.Equal("O Campo 'Email' n�o � v�lido.", result.Message);
        }

        [Fact(DisplayName = "Validando se Password do usu�rio para cadastro esta vazio")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenPasswordIsEmpty()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithPasswordIsEmpty());

            //Assert
            Assert.Equal("O Campo 'Password' � Obrigat�rio.", result.Message);
        }

        [Fact(DisplayName = "Validando se Password do usu�rio para cadastro ultrapasssou o limite de caracteres")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenPasswordMaxLength()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithPasswordMaxLength());

            //Assert
            Assert.Equal("20 � o tamanho m�ximo para o campo 'Password'", result.Message);
        }

        [Fact(DisplayName = "Validando se RePassword do usu�rio para cadastro esta vazio")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenRePasswordIsEmpty()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithRePasswordIsEmpty());

            //Assert
            Assert.Equal("O Campo 'RePassword' � Obrigat�rio.", result.Message);
        }

        [Fact(DisplayName = "Validando se RePassword do usu�rio para cadastro � igual ao campo Password")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenRePasswordNoMatch()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithPasswordNoMatch());

            //Assert
            Assert.Equal("As senhas n�o conferem!", result.Message);
        }

        [Fact(DisplayName = "Validando se IdUsuarioInclusao do usu�rio para cadastro  ultrapasssou o limite de caracteres")]
        [Trait("Categoria", "Validando Usu�rio")]
        public void ValidateDomain_ShouldThrowException_WhenIdUsuarioInclusaoMaxLength()
        {
            // Act
            var result = Assert.Throws<DomainException>(() => _usuarioTestsFixture.CreateUserWithIdUsuarioInclusaoMaxLength());

            //Assert
            Assert.Equal("450 � o tamanho m�ximo para o campo 'IdUsuarioInclusao'", result.Message);
        }
    }
}