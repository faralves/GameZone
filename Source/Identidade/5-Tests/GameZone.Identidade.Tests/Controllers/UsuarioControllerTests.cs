using GameZone.Identidade.API.Controllers;
using GameZone.Identidade.Application.DTOs.Response;
using GameZone.Identidade.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace GameZone.Identidade.Tests.Api.Controllers
{
    public class UsuarioControllerTests 
    {
        private UsuarioController _controller;
        private Mock<IUsuarioApplication> _usuarioApplicationMock;
        private Mock<ILogger<UsuarioController>> _loggerMock;

        public UsuarioControllerTests()
        {
            // Configurar o mock da application
            _usuarioApplicationMock = new Mock<IUsuarioApplication>();

            // Configurar o mock do logger
            _loggerMock = new Mock<ILogger<UsuarioController>>();

            // Inicializar o controller com o mock da application e do logger
            _controller = new UsuarioController(_usuarioApplicationMock.Object, _loggerMock.Object);
        }

        [Fact(DisplayName = "Validando se a classe esta correta")]
        [Trait("Categoria", "Validando Login Usuário")]
        public async Task GetIdUsuario_ReturnsOkResultWithData()
        {
            // Arrange
            var usuario = new UsuarioDto() 
            {
                Name = "Admin User",
                CpfCnpj = "12345678901",
                DataNascimento = new DateTime(2023, 10, 28),
                IsAdministrator = true,
                IdUsuarioInclusao = string.Empty
            };

            Task<UsuarioDto> usuarioDto = Task.FromResult(usuario);

            var id = Guid.Parse("1f1d2bea-e60d-408d-a9e0-ecb341fc656b");

            _usuarioApplicationMock.Setup(s => s.GetUser(id)).Returns(usuarioDto);

            // Act
            var result = await _controller.GetUser(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(usuario, okResult.Value);
        }
    }
}