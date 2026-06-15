using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Handlers;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using FluentAssertions;
using Moq;

namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.ProfissionalSaudeHandler
{
    public class GetProfissionalSaudeByIdQueryHandlerTests
    {
        private readonly Mock<IProfissionalSaudeRepository> _profissionalSaudeRepositoryMock;
        private readonly GetProfissionalSaudeByIdQueryHandler _handler;

        public GetProfissionalSaudeByIdQueryHandlerTests()
        {
            _profissionalSaudeRepositoryMock = new Mock<IProfissionalSaudeRepository>();

            _handler = new GetProfissionalSaudeByIdQueryHandler(
                _profissionalSaudeRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Profissional_Quando_Existir()
        {
            // Arrange
            var profissionalId = Guid.NewGuid();

            var profissional = new ProfissionalSaude(
                "Dra. Ana Souza",
                "Cardiologia",
                "CRM123456");

            var query = new GetProfissionalSaudeByIdQuery
            {
                Id = profissionalId
            };

            _profissionalSaudeRepositoryMock
                .Setup(x => x.ObterPorIdAsync(profissionalId))
                .ReturnsAsync(profissional);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(profissional);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.ObterPorIdAsync(profissionalId),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Profissional_Nao_Existir()
        {
            // Arrange
            var profissionalId = Guid.NewGuid();

            var query = new GetProfissionalSaudeByIdQuery
            {
                Id = profissionalId
            };

            _profissionalSaudeRepositoryMock
                .Setup(x => x.ObterPorIdAsync(profissionalId))
                .ReturnsAsync((ProfissionalSaude?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _profissionalSaudeRepositoryMock.Verify(
                x => x.ObterPorIdAsync(profissionalId),
                Times.Once);
        }
    }
}
