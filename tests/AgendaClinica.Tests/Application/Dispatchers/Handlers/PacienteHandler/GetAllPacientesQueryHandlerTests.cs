using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Handlers;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using FluentAssertions;
using MediatR;
using Moq;

namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.PacienteHandler
{
    public class GetAllPacientesQueryHandlerTests
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GetAllPacientesQueryHandler _handler;

        public GetAllPacientesQueryHandlerTests()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new GetAllPacientesQueryHandler(
                _pacienteRepositoryMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Todos_Os_Pacientes()
        {
            // Arrange
            var pacientes = new List<Paciente>
        {
            new Paciente("João da Silva", "12345678900", "joao@email.com", "11999999999"),
            new Paciente("Maria Oliveira", "98765432100", "maria@email.com", "11888888888")
        };

            var query = new GetAllPacientesQuery();

            _pacienteRepositoryMock
                .Setup(x => x.ObterTodosAsync())
                .ReturnsAsync(pacientes);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(pacientes);

            _pacienteRepositoryMock.Verify(
                x => x.ObterTodosAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Pacientes()
        {
            // Arrange
            var query = new GetAllPacientesQuery();

            _pacienteRepositoryMock
                .Setup(x => x.ObterTodosAsync())
                .ReturnsAsync(new List<Paciente>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _pacienteRepositoryMock.Verify(
                x => x.ObterTodosAsync(),
                Times.Once);
        }
    }
}
