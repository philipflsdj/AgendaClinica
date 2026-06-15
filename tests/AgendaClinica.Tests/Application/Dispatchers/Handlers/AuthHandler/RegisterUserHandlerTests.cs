using AgendaClinica.Application.Configurations.DTOs.Auth;
using AgendaClinica.Application.Configurations.Interfaces;
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
    public class RegisterUserHandlerTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IPasswordHasherService> _passwordHasherServiceMock;
        private readonly Mock<IValidator<RegisterUserCommand>> _validatorMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;

        private readonly RegisterUserHandler _handler;

        public RegisterUserHandlerTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _passwordHasherServiceMock = new Mock<IPasswordHasherService>();
            _validatorMock = new Mock<IValidator<RegisterUserCommand>>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();

            _handler = new RegisterUserHandler(
                _usuarioRepositoryMock.Object,
                _tokenServiceMock.Object,
                _passwordHasherServiceMock.Object,
                _validatorMock.Object,
                _mediatorHandlerMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Falha_Quando_Command_For_Invalido()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                Nome = "",
                Email = "",
                Password = ""
            };

            var validationResult = new ValidationResult(new[]
            {
            new ValidationFailure("Nome", "Nome é obrigatório."),
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

            _usuarioRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<Usuario>()),
                Times.Never);

            _tokenServiceMock.Verify(
                x => x.GenerateToken(It.IsAny<Usuario>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Falha_Quando_Email_Ja_Existir()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                Nome = "João",
                Email = "joao@email.com",
                Password = "123456"
            };

            var usuarioExistente = new Usuario(
                "João Existente",
                command.Email,
                "hash-existente",
                "USER");

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _usuarioRepositoryMock
                .Setup(x => x.ObterPorEmailAsync(command.Email))
                .ReturnsAsync(usuarioExistente);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();

            _passwordHasherServiceMock.Verify(
                x => x.HashPassword(It.IsAny<string>()),
                Times.Never);

            _usuarioRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<Usuario>()),
                Times.Never);

            _tokenServiceMock.Verify(
                x => x.GenerateToken(It.IsAny<Usuario>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Registrar_Usuario_Quando_Dados_Forem_Validos()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                Nome = "Maria",
                Email = "maria@email.com",
                Password = "123456"
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _usuarioRepositoryMock
                .Setup(x => x.ObterPorEmailAsync(command.Email))
                .ReturnsAsync((Usuario?)null);

            _passwordHasherServiceMock
                .Setup(x => x.HashPassword(command.Password))
                .Returns("senha-hasheada");

            _usuarioRepositoryMock
                .Setup(x => x.AdicionarAsync(It.IsAny<Usuario>()))
                .Returns(Task.CompletedTask);

            _tokenServiceMock
                .Setup(x => x.GenerateToken(It.IsAny<Usuario>()))
                .Returns("token-jwt-gerado");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Token.Should().Be("token-jwt-gerado");
            result.Data.Nome.Should().Be(command.Nome);
            result.Data.Email.Should().Be(command.Email);

            _usuarioRepositoryMock.Verify(
                x => x.ObterPorEmailAsync(command.Email),
                Times.Once);

            _passwordHasherServiceMock.Verify(
                x => x.HashPassword(command.Password),
                Times.Once);

            _usuarioRepositoryMock.Verify(
                x => x.AdicionarAsync(It.Is<Usuario>(u =>
                    u.Nome == command.Nome &&
                    u.Email == command.Email &&
                    u.SenhaHash == "senha-hasheada" &&
                    u.Role == "USER")),
                Times.Once);

            _tokenServiceMock.Verify(
                x => x.GenerateToken(It.Is<Usuario>(u =>
                    u.Nome == command.Nome &&
                    u.Email == command.Email &&
                    u.SenhaHash == "senha-hasheada" &&
                    u.Role == "USER")),
                Times.Once);
        }
    }
}
