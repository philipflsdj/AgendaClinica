using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using FluentAssertions;
using MediatR;
using Moq;


namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.ConsultaHandler
{
    public class GetAgendaProfissionalHandlerTests
    {
        private readonly Mock<IConsultaRepository> _consultaRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GetAgendaProfissionalHandler _handler;

        public GetAgendaProfissionalHandlerTests()
        {
            _consultaRepositoryMock = new Mock<IConsultaRepository>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new GetAgendaProfissionalHandler(
                _consultaRepositoryMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Agenda_Do_Profissional_Com_Sucesso()
        {
            // Arrange
            var profissionalId = Guid.NewGuid();
            var data = new DateTime(2026, 6, 15);

            var consultas = new List<Consulta>
        {
            new Consulta(Guid.NewGuid(), profissionalId, new DateTime(2026, 6, 15, 8, 0, 0)),
            new Consulta(Guid.NewGuid(), profissionalId, new DateTime(2026, 6, 15, 9, 0, 0))
        };

            var query = new GetAgendaProfissionalQuery
            {
                ProfissionalId = profissionalId,
                Data = data
            };

            _consultaRepositoryMock
                .Setup(x => x.ObterAgendaProfissionalAsync(profissionalId, data))
                .ReturnsAsync(consultas);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.Data.Should().BeEquivalentTo(consultas);

            _consultaRepositoryMock.Verify(
                x => x.ObterAgendaProfissionalAsync(profissionalId, data),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Falha_Quando_Repositorio_Lancar_Excecao()
        {
            // Arrange
            var profissionalId = Guid.NewGuid();
            var data = new DateTime(2026, 6, 15);

            var query = new GetAgendaProfissionalQuery
            {
                ProfissionalId = profissionalId,
                Data = data
            };

            _consultaRepositoryMock
                .Setup(x => x.ObterAgendaProfissionalAsync(profissionalId, data))
                .ThrowsAsync(new Exception("Erro no banco de dados"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();

            _consultaRepositoryMock.Verify(
                x => x.ObterAgendaProfissionalAsync(profissionalId, data),
                Times.Once);
        }
    }
}
