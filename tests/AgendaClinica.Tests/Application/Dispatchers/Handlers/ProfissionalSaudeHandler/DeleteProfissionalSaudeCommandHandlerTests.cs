using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Handlers;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using FluentAssertions;
using Moq;

namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.ProfissionalSaudeHandler
{
    public class DeleteProfissionalSaudeCommandHandlerTests
    {
        private readonly Mock<IProfissionalSaudeRepository> _profissionalSaudeRepositoryMock;
        private readonly DeleteProfissionalSaudeCommandHandler _handler;

        public DeleteProfissionalSaudeCommandHandlerTests()
        {
            _profissionalSaudeRepositoryMock = new Mock<IProfissionalSaudeRepository>();

            _handler = new DeleteProfissionalSaudeCommandHandler(
                _profissionalSaudeRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_False_Quando_Profissional_Nao_Existir()
        {
            // Arrange
            var profissionalId = Guid.NewGuid();

            var command = new DeleteProfissionalSaudeCommand
            {
                Id = profissionalId
            };

            _profissionalSaudeRepositoryMock
                .Setup(x => x.ObterPorIdAsync(profissionalId))
                .ReturnsAsync((ProfissionalSaude?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();

            _profissionalSaudeRepositoryMock.Verify(
                x => x.RemoverAsync(It.IsAny<Guid>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Remover_Profissional_Quando_Existir()
        {
            // Arrange
            var profissionalId = Guid.NewGuid();

            var profissional = new ProfissionalSaude(
                "Dra. Ana Souza",
                "Cardiologia",
                "CRM123456");

            var command = new DeleteProfissionalSaudeCommand
            {
                Id = profissionalId
            };

            _profissionalSaudeRepositoryMock
                .Setup(x => x.ObterPorIdAsync(profissionalId))
                .ReturnsAsync(profissional);

            _profissionalSaudeRepositoryMock
                .Setup(x => x.RemoverAsync(profissionalId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();

            _profissionalSaudeRepositoryMock.Verify(
                x => x.ObterPorIdAsync(profissionalId),
                Times.Once);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.RemoverAsync(profissionalId),
                Times.Once);
        }
    }
}
