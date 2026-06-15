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

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers
{
    public class AddConsultaHandlerTests
    {
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IValidator<AddConsultaCommand>> _validatorMock;
        private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;
        private readonly Mock<IConsultaRepository> _consultaRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly AddConsultaHandler _handler;

        public AddConsultaHandlerTests()
        {
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _validatorMock = new Mock<IValidator<AddConsultaCommand>>();
            _localizerMock = new Mock<IStringLocalizer<SharedResource>>();
            _consultaRepositoryMock = new Mock<IConsultaRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new AddConsultaHandler(
                _mediatorHandlerMock.Object,
                _validatorMock.Object,
                _localizerMock.Object,
                _consultaRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Command_For_Invalido()
        {
            // Arrange
            var consulta = CriarConsultaRequestValida();

            var command = new AddConsultaCommand
            {
                Consulta = consulta
            };

            var validationResult = new ValidationResult(new[]
            {
            new ValidationFailure("Consulta", "Consulta inválida.")
        });

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _consultaRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<Consulta>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Paciente_Ja_Possuir_Consulta_Com_Profissional_No_Dia()
        {
            // Arrange
            var consulta = CriarConsultaRequestValida();

            var command = new AddConsultaCommand
            {
                Consulta = consulta
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _consultaRepositoryMock
                .Setup(x => x.PacientePossuiConsultaComProfissionalNoDiaAsync(
                    consulta.PacienteId,
                    consulta.ProfissionalId,
                    consulta.InicioEm))
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

            _consultaRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<Consulta>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Adicionar_Consulta_Quando_Command_For_Valido()
        {
            // Arrange
            var consultaRequest = CriarConsultaRequestValida();

            var consultaMapeada = new Consulta(
                consultaRequest.PacienteId,
                consultaRequest.ProfissionalId,
                consultaRequest.InicioEm);

            var command = new AddConsultaCommand
            {
                Consulta = consultaRequest
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _consultaRepositoryMock
                .Setup(x => x.PacientePossuiConsultaComProfissionalNoDiaAsync(
                    consultaRequest.PacienteId,
                    consultaRequest.ProfissionalId,
                    consultaRequest.InicioEm))
                .ReturnsAsync(false);

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
            result.Should().BeEquivalentTo(consultaMapeada);

            _consultaRepositoryMock.Verify(
                x => x.PacientePossuiConsultaComProfissionalNoDiaAsync(
                    consultaRequest.PacienteId,
                    consultaRequest.ProfissionalId,
                    consultaRequest.InicioEm),
                Times.Once);

            _consultaRepositoryMock.Verify(
                x => x.AdicionarAsync(consultaMapeada),
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
