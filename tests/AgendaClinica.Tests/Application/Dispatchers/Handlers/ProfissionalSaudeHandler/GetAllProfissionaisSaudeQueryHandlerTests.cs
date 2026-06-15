using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Handlers;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using FluentAssertions;
using Moq;

namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.ProfissionalSaudeHandler
{
    public class GetAllProfissionaisSaudeQueryHandlerTests
    {
        private readonly Mock<IProfissionalSaudeRepository> _profissionalSaudeRepositoryMock;
        private readonly GetAllProfissionaisSaudeQueryHandler _handler;

        public GetAllProfissionaisSaudeQueryHandlerTests()
        {
            _profissionalSaudeRepositoryMock = new Mock<IProfissionalSaudeRepository>();

            _handler = new GetAllProfissionaisSaudeQueryHandler(
                _profissionalSaudeRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Todos_Os_Profissionais()
        {
            // Arrange
            var profissionais = new List<ProfissionalSaude>
        {
            new ProfissionalSaude("Dra. Ana Souza", "Cardiologia", "CRM123456"),
            new ProfissionalSaude("Dr. Carlos Lima", "Ortopedia", "CRM987654")
        };

            var query = new GetAllProfissionaisSaudeQuery();

            _profissionalSaudeRepositoryMock
                .Setup(x => x.ObterTodosAsync())
                .ReturnsAsync(profissionais);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(profissionais);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.ObterTodosAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Profissionais()
        {
            // Arrange
            var query = new GetAllProfissionaisSaudeQuery();

            _profissionalSaudeRepositoryMock
                .Setup(x => x.ObterTodosAsync())
                .ReturnsAsync(new List<ProfissionalSaude>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _profissionalSaudeRepositoryMock.Verify(
                x => x.ObterTodosAsync(),
                Times.Once);
        }
    }
}
