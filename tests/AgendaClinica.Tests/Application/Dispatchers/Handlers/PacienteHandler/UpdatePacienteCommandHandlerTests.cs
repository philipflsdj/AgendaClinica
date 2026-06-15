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
    public class UpdatePacienteCommandHandlerTests
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly Mock<IValidator<UpdatePacienteCommand>> _validatorMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly UpdatePacienteCommandHandler _handler;

        public UpdatePacienteCommandHandlerTests()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _validatorMock = new Mock<IValidator<UpdatePacienteCommand>>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _localizerMock = new Mock<IStringLocalizer<SharedResource>>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdatePacienteCommandHandler(
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

            var command = new UpdatePacienteCommand
            {
                Id = Guid.NewGuid(),
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
                x => x.ObterPorIdAsync(It.IsAny<Guid>()),
                Times.Never);

            _pacienteRepositoryMock.Verify(
                x => x.AtualizarAsync(It.IsAny<Paciente>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Paciente_Nao_Existir()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();

            var pacienteRequest = CriarPacienteRequestValido();

            var command = new UpdatePacienteCommand
            {
                Id = pacienteId,
                Item = pacienteRequest
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pacienteRepositoryMock
                .Setup(x => x.ObterPorIdAsync(pacienteId))
                .ReturnsAsync((Paciente?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _mediatorHandlerMock.Verify(
                x => x.PublicarNotificacao(It.IsAny<Notificacao>()),
                Times.Once);

            _pacienteRepositoryMock.Verify(
                x => x.AtualizarAsync(It.IsAny<Paciente>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Atualizar_Paciente_Quando_Command_For_Valido()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();

            var pacienteExistente = new Paciente(
                "João da Silva",
                "12345678900",
                "joao@email.com",
                "11999999999");

            var pacienteRequest = CriarPacienteRequestValido();

            var pacienteMapeado = new Paciente(
                pacienteRequest.nomeCompleto,
                pacienteRequest.documento,
                pacienteRequest.email,
                pacienteRequest.telefone);

            var command = new UpdatePacienteCommand
            {
                Id = pacienteId,
                Item = pacienteRequest
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pacienteRepositoryMock
                .Setup(x => x.ObterPorIdAsync(pacienteId))
                .ReturnsAsync(pacienteExistente);

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
                x => x.ObterPorIdAsync(pacienteId),
                Times.Once);

            _pacienteRepositoryMock.Verify(
                x => x.AtualizarAsync(It.IsAny<Paciente>()),
                Times.Once);
        }

        private static CriarPaciente CriarPacienteRequestValido()
        {
            return new CriarPaciente
            {
                nomeCompleto = "Maria Oliveira",
                documento = "98765432100",
                email = "maria@email.com",
                telefone = "11888888888"
            };
        }
    }
}
