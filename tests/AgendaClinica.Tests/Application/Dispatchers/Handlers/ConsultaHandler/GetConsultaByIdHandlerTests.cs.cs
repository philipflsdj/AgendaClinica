using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using FluentAssertions;
using MediatR;
using Moq;

namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.ConsultaHandler
{
    public class GetConsultaByIdHandlerTests
    {
        private readonly Mock<IConsultaRepository> _consultaRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GetConsultaByIdHandler _handler;

        public GetConsultaByIdHandlerTests()
        {
            _consultaRepositoryMock = new Mock<IConsultaRepository>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new GetConsultaByIdHandler(
                _mediatorMock.Object,
                _consultaRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Consulta_Quando_Existir()
        {
            // Arrange
            var consultaId = Guid.NewGuid();

            var consulta = new Consulta(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateTime(2026, 6, 15, 10, 0, 0));

            var query = new GetConsultaByIdQuery
            {
                Id = consultaId
            };

            _consultaRepositoryMock
                .Setup(x => x.ObterPorIdAsync(consultaId))
                .ReturnsAsync(consulta);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(consulta);

            _consultaRepositoryMock.Verify(
                x => x.ObterPorIdAsync(consultaId),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Consulta_Nao_Existir()
        {
            // Arrange
            var consultaId = Guid.NewGuid();

            var query = new GetConsultaByIdQuery
            {
                Id = consultaId
            };

            _consultaRepositoryMock
                .Setup(x => x.ObterPorIdAsync(consultaId))
                .ReturnsAsync((Consulta?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _consultaRepositoryMock.Verify(
                x => x.ObterPorIdAsync(consultaId),
                Times.Once);
        }
    }
}
