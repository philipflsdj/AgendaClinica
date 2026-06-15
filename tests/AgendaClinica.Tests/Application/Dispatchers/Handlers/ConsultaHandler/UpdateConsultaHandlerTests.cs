using AgendaClinica.Application.Configurations.DTOs.Consultas;
using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using Application;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Moq;

namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.ConsultaHandler
{
    public class UpdateConsultaHandlerTests
    {
        private readonly Mock<IConsultaRepository> _consultaRepositoryMock;
        private readonly Mock<IValidator<UpdateConsultaCommand>> _validatorMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly UpdateConsultaHandler _handler;

        public UpdateConsultaHandlerTests()
        {
            _consultaRepositoryMock = new Mock<IConsultaRepository>();
            _validatorMock = new Mock<IValidator<UpdateConsultaCommand>>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _localizerMock = new Mock<IStringLocalizer<SharedResource>>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateConsultaHandler(
                _consultaRepositoryMock.Object,
                _validatorMock.Object,
                _mediatorHandlerMock.Object,
                _localizerMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Command_For_Invalido()
        {
            // Arrange
            var command = new UpdateConsultaCommand
            {
                Id = Guid.NewGuid(),
                Item = CriarConsultaRequestValida()
            };

            var validationResult = new ValidationResult(new[]
            {
            new ValidationFailure("Item", "Consulta inválida.")
        });

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _localizerMock
                .Setup(x => x["Consulta inválida."])
                .Returns(new LocalizedString("Consulta inválida.", "Consulta inválida."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _mediatorHandlerMock.Verify(
                x => x.PublicarNotificacao(It.IsAny<Notificacao>()),
                Times.Once);

            _consultaRepositoryMock.Verify(
                x => x.ObterPorIdAsync(It.IsAny<Guid>()),
                Times.Never);

            _consultaRepositoryMock.Verify(
                x => x.AtualizarAsync(It.IsAny<Consulta>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Consulta_Nao_Existir()
        {
            // Arrange
            var consultaId = Guid.NewGuid();

            var command = new UpdateConsultaCommand
            {
                Id = consultaId,
                Item = CriarConsultaRequestValida()
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _consultaRepositoryMock
                .Setup(x => x.ObterPorIdAsync(consultaId))
                .ReturnsAsync((Consulta?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _consultaRepositoryMock.Verify(
                x => x.ObterPorIdAsync(consultaId),
                Times.Once);

            _consultaRepositoryMock.Verify(
                x => x.AtualizarAsync(It.IsAny<Consulta>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Atualizar_Consulta_Quando_Command_For_Valido()
        {
            // Arrange
            var consultaId = Guid.NewGuid();

            var consultaExistente = new Consulta(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateTime(2026, 6, 15, 9, 0, 0));

            var consultaRequest = CriarConsultaRequestValida();

            var consultaMapeada = new Consulta(
                consultaRequest.PacienteId,
                consultaRequest.ProfissionalId,
                consultaRequest.InicioEm);

            var command = new UpdateConsultaCommand
            {
                Id = consultaId,
                Item = consultaRequest
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _consultaRepositoryMock
                .Setup(x => x.ObterPorIdAsync(consultaId))
                .ReturnsAsync(consultaExistente);

            _mapperMock
                .Setup(x => x.Map<Consulta>(consultaRequest))
                .Returns(consultaMapeada);

            _consultaRepositoryMock
                .Setup(x => x.AdicionarAsync(consultaMapeada))
                .ReturnsAsync(consultaMapeada);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();

            result!.PacienteId.Should().Be(consultaMapeada.PacienteId);
            result.ProfissionalId.Should().Be(consultaMapeada.ProfissionalId);
            result.InicioEm.Should().Be(consultaMapeada.InicioEm);

            _consultaRepositoryMock.Verify(
                x => x.ObterPorIdAsync(consultaId),
                Times.Once);

            _consultaRepositoryMock.Verify(
                x => x.AtualizarAsync(It.IsAny<Consulta>()),
                Times.Once);
        }

        private static CriarConsultaRequest CriarConsultaRequestValida()
        {
            return new CriarConsultaRequest
            {
                PacienteId = Guid.NewGuid(),
                ProfissionalId = Guid.NewGuid(),
                InicioEm = new DateTime(2026, 6, 15, 9, 0, 0)
            };
        }
    }
}
