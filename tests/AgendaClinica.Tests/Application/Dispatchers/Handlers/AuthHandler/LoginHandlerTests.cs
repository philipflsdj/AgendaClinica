using AgendaClinica.Application.Configurations.DTOs.Auth;
using AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Handler;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Contracts.Services;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;


namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.AuthHandler
{
    public class LoginHandlerTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IValidator<LoginCommand>> _validatorMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IPasswordHasherService> _passwordHasherServiceMock;

        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _validatorMock = new Mock<IValidator<LoginCommand>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _passwordHasherServiceMock = new Mock<IPasswordHasherService>();

            _handler = new LoginHandler(
                _usuarioRepositoryMock.Object,
                _validatorMock.Object,
                _tokenServiceMock.Object,
                _passwordHasherServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Falha_Quando_Command_For_Invalido()
        {
            // Arrange
            var command = new LoginCommand
            {
                Email = "",
                Password = ""
            };

            var validationResult = new ValidationResult(new[]
            {
            new ValidationFailure("Email", "E-mail é obrigatório."),
            new ValidationFailure("Password", "Senha é obrigatória.")
        });

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();

            _usuarioRepositoryMock.Verify(
                x => x.ObterPorEmailAsync(It.IsAny<string>()),
                Times.Never);

            _tokenServiceMock.Verify(
                x => x.GenerateToken(It.IsAny<Usuario>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Falha_Quando_Usuario_Nao_Existir()
        {
            // Arrange
            var command = new LoginCommand
            {
                Email = "usuario@email.com",
                Password = "123456"
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _usuarioRepositoryMock
                .Setup(x => x.ObterPorEmailAsync(command.Email))
                .ReturnsAsync((Usuario?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();

            _tokenServiceMock.Verify(
                x => x.GenerateToken(It.IsAny<Usuario>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Falha_Quando_Senha_For_Invalida()
        {
            // Arrange
            var command = new LoginCommand
            {
                Email = "admin@email.com",
                Password = "senha-errada"
            };

            var usuario = new Usuario(
                "Admin",
                "admin@email.com",
                "hash-da-senha",
                "ADMIN");

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _usuarioRepositoryMock
                .Setup(x => x.ObterPorEmailAsync(command.Email))
                .ReturnsAsync(usuario);

            _passwordHasherServiceMock
                .Setup(x => x.VerifyPassword(command.Password, usuario.SenhaHash))
                .Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();

            _tokenServiceMock.Verify(
                x => x.GenerateToken(It.IsAny<Usuario>()),
                Times.Never);
        }
    }
}
