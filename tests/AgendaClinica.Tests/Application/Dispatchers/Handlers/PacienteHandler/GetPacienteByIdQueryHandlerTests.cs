using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Handlers;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using FluentAssertions;
using MediatR;
using Moq;


namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.PacienteHandler
{
    public class GetPacienteByIdQueryHandlerTests
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GetPacienteByIdQueryHandler _handler;

        public GetPacienteByIdQueryHandlerTests()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new GetPacienteByIdQueryHandler(
                _pacienteRepositoryMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Paciente_Quando_Existir()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();

            var paciente = new Paciente(
                "João da Silva",
                "12345678900",
                "joao@email.com",
                "11999999999");

            var query = new GetPacienteByIdQuery
            {
                Id = pacienteId
            };

            _pacienteRepositoryMock
                .Setup(x => x.ObterPorIdAsync(pacienteId))
                .ReturnsAsync(paciente);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(paciente);

            _pacienteRepositoryMock.Verify(
                x => x.ObterPorIdAsync(pacienteId),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Paciente_Nao_Existir()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();

            var query = new GetPacienteByIdQuery
            {
                Id = pacienteId
            };

            _pacienteRepositoryMock
                .Setup(x => x.ObterPorIdAsync(pacienteId))
                .ReturnsAsync((Paciente?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _pacienteRepositoryMock.Verify(
                x => x.ObterPorIdAsync(pacienteId),
                Times.Once);
        }
    }
}
