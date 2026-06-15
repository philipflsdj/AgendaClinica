using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using FluentAssertions;
using MediatR;
using Moq;


namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.ConsultaHandler
{
    public class GetAllConsultasHandlerTests
    {
        private readonly Mock<IConsultaRepository> _consultaRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GetAllConsultasHandler _handler;

        public GetAllConsultasHandlerTests()
        {
            _consultaRepositoryMock = new Mock<IConsultaRepository>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new GetAllConsultasHandler(
                _consultaRepositoryMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Todas_As_Consultas()
        {
            // Arrange
            var consultas = new List<Consulta?>
        {
            new Consulta(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2026, 6, 15, 8, 0, 0)),
            new Consulta(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2026, 6, 15, 9, 0, 0)),
            new Consulta(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2026, 6, 15, 10, 0, 0))
        };

            var query = new GetAllConsultasQuery();

            _consultaRepositoryMock
                .Setup(x => x.ObterTodasAsync())
                .ReturnsAsync(consultas);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(consultas);

            _consultaRepositoryMock.Verify(
                x => x.ObterTodasAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Consultas()
        {
            // Arrange
            var query = new GetAllConsultasQuery();

            _consultaRepositoryMock
                .Setup(x => x.ObterTodasAsync())
                .ReturnsAsync(new List<Consulta?>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _consultaRepositoryMock.Verify(
                x => x.ObterTodasAsync(),
                Times.Once);
        }
    }
}
