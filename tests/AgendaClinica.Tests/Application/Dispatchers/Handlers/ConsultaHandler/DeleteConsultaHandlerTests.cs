using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using FluentAssertions;
using Moq;

namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.ConsultaHandler
{
    public class DeleteConsultaHandlerTests
    {
        private readonly Mock<IConsultaRepository> _consultaRepositoryMock;
        private readonly DeleteConsultaHandler _handler;

        public DeleteConsultaHandlerTests()
        {
            _consultaRepositoryMock = new Mock<IConsultaRepository>();
            _handler = new DeleteConsultaHandler(_consultaRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_False_Quando_Consulta_Nao_Existir()
        {
            // Arrange
            var consultaId = Guid.NewGuid();

            var command = new DeleteConsultaCommand
            {
                Id = consultaId
            };

            _consultaRepositoryMock
                .Setup(x => x.ObterPorIdAsync(consultaId))
                .ReturnsAsync((Consulta?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();

            _consultaRepositoryMock.Verify(
                x => x.CancelarAsync(It.IsAny<Guid>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Cancelar_Consulta_Quando_Consulta_Existir()
        {
            // Arrange
            var consultaId = Guid.NewGuid();

            var consulta = new Consulta(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateTime(2026, 6, 15, 9, 0, 0));

            var command = new DeleteConsultaCommand
            {
                Id = consultaId
            };

            _consultaRepositoryMock
                .Setup(x => x.ObterPorIdAsync(consultaId))
                .ReturnsAsync(consulta);

            _consultaRepositoryMock
                .Setup(x => x.CancelarAsync(consultaId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();

            _consultaRepositoryMock.Verify(
                x => x.ObterPorIdAsync(consultaId),
                Times.Once);

            _consultaRepositoryMock.Verify(
                x => x.CancelarAsync(consultaId),
                Times.Once);
        }
    }
}
