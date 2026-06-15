using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Handlers;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using FluentAssertions;
using Moq;

namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.PacienteHandler
{
    public class DeletePacienteCommandHandlerTests
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly DeletePacienteCommandHandler _handler;

        public DeletePacienteCommandHandlerTests()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();

            _handler = new DeletePacienteCommandHandler(
                _pacienteRepositoryMock.Object,
                _mediatorHandlerMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_False_Quando_Paciente_Nao_Existir()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();

            var command = new DeletePacienteCommand
            {
                Id = pacienteId
            };

            _pacienteRepositoryMock
                .Setup(x => x.ObterPorIdAsync(pacienteId))
                .ReturnsAsync((Paciente?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();

            _pacienteRepositoryMock.Verify(
                x => x.RemoverAsync(It.IsAny<Guid>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Remover_Paciente_Quando_Paciente_Existir()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();

            var paciente = new Paciente(
                "João da Silva",
                "12345678900",
                "joao@email.com",
                "11999999999");

            var command = new DeletePacienteCommand
            {
                Id = pacienteId
            };

            _pacienteRepositoryMock
                .Setup(x => x.ObterPorIdAsync(pacienteId))
                .ReturnsAsync(paciente);

            _pacienteRepositoryMock
                .Setup(x => x.RemoverAsync(pacienteId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();

            _pacienteRepositoryMock.Verify(
                x => x.ObterPorIdAsync(pacienteId),
                Times.Once);

            _pacienteRepositoryMock.Verify(
                x => x.RemoverAsync(pacienteId),
                Times.Once);
        }
    }
}
