using AgendaClinica.Application.Configurations.DTOs.Pacientes;
using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Handlers;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using Application;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Moq;

namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.PacienteHandler
{
    public class AddPacienteCommandHandlerTests
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly Mock<IValidator<AddPacienteCommand>> _validatorMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly AddPacienteCommandHandler _handler;

        public AddPacienteCommandHandlerTests()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _validatorMock = new Mock<IValidator<AddPacienteCommand>>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _localizerMock = new Mock<IStringLocalizer<SharedResource>>();
            _mapperMock = new Mock<IMapper>();

            _handler = new AddPacienteCommandHandler(
                _pacienteRepositoryMock.Object,
                _validatorMock.Object,
                _mediatorHandlerMock.Object,
                _localizerMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Command_For_Invalido()
        {
            // Arrange
            var pacienteRequest = CriarPacienteRequestValido();

            var command = new AddPacienteCommand
            {
                Item = pacienteRequest
            };

            var validationResult = new ValidationResult(new[]
            {
            new ValidationFailure("NomeCompleto", "Nome é obrigatório.")
        });

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _localizerMock
                .Setup(x => x["Nome é obrigatório."])
                .Returns(new LocalizedString("Nome é obrigatório.", "Nome é obrigatório."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _mediatorHandlerMock.Verify(
                x => x.PublicarNotificacao(It.IsAny<Notificacao>()),
                Times.Once);

            _pacienteRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<Paciente>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Documento_Ja_Existir()
        {
            // Arrange
            var pacienteRequest = CriarPacienteRequestValido();

            var command = new AddPacienteCommand
            {
                Item = pacienteRequest
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pacienteRepositoryMock
                .Setup(x => x.DocumentoJaExisteAsync(pacienteRequest.documento))
                .ReturnsAsync(true);

            _localizerMock
                .Setup(x => x["NotFound"])
                .Returns(new LocalizedString("NotFound", "NotFound"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _mediatorHandlerMock.Verify(
                x => x.PublicarNotificacao(It.IsAny<Notificacao>()),
                Times.Once);

            _pacienteRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<Paciente>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Adicionar_Paciente_Quando_Command_For_Valido()
        {
            // Arrange
            var pacienteRequest = CriarPacienteRequestValido();

            var pacienteMapeado = new Paciente(
                pacienteRequest.nomeCompleto,
                pacienteRequest.documento,
                pacienteRequest.email,
                pacienteRequest.telefone);

            var command = new AddPacienteCommand
            {
                Item = pacienteRequest
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pacienteRepositoryMock
                .Setup(x => x.DocumentoJaExisteAsync(pacienteRequest.documento))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x => x.Map<Paciente>(pacienteRequest))
                .Returns(pacienteMapeado);

            _pacienteRepositoryMock
                .Setup(x => x.AdicionarAsync(It.IsAny<Paciente>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.NomeCompleto.Should().Be(pacienteRequest.nomeCompleto);
            result.Documento.Should().Be(pacienteRequest.documento);
            result.Email.Should().Be(pacienteRequest.email);
            result.Telefone.Should().Be(pacienteRequest.telefone);

            _pacienteRepositoryMock.Verify(
                x => x.DocumentoJaExisteAsync(pacienteRequest.documento),
                Times.Once);

            _pacienteRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<Paciente>()),
                Times.Once);
        }

        private static CriarPaciente CriarPacienteRequestValido()
        {
            return new CriarPaciente
            {
                nomeCompleto = "João da Silva",
                documento = "12345678900",
                email = "joao@email.com",
                telefone = "11999999999"
            };
        }
    }
}
