using AgendaClinica.Application.Configurations.DTOs.ProfissionaisSaude;
using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Handlers;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using Application;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Moq;

namespace AgendaClinica.Tests.Application.Dispatchers.Handlers.ProfissionalSaudeHandler
{
    public class UpdateProfissionalSaudeCommandHandlerTests
    {
        private readonly Mock<IProfissionalSaudeRepository> _profissionalSaudeRepositoryMock;
        private readonly Mock<IValidator<UpdateProfissionalSaudeCommand>> _validatorMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly UpdateProfissionalSaudeCommandHandler _handler;

        public UpdateProfissionalSaudeCommandHandlerTests()
        {
            _profissionalSaudeRepositoryMock = new Mock<IProfissionalSaudeRepository>();
            _validatorMock = new Mock<IValidator<UpdateProfissionalSaudeCommand>>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _localizerMock = new Mock<IStringLocalizer<SharedResource>>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateProfissionalSaudeCommandHandler(
                _profissionalSaudeRepositoryMock.Object,
                _validatorMock.Object,
                _mediatorHandlerMock.Object,
                _localizerMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Command_For_Invalido()
        {
            // Arrange
            var profissionalRequest = CriarProfissionalSaudeRequestValido();

            var command = new UpdateProfissionalSaudeCommand
            {
                Id = Guid.NewGuid(),
                Item = profissionalRequest
            };

            var validationResult = new ValidationResult(new[]
            {
            new ValidationFailure("NomeCompleto", "Nome é obrigatório.")
        });

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _localizerMock
                .Setup(x => x["Nome é obrigatório."])
                .Returns(new LocalizedString("Nome é obrigatório.", "Nome é obrigatório."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _mediatorHandlerMock.Verify(
                x => x.PublicarNotificacao(It.IsAny<Notificacao>()),
                Times.Once);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.ObterPorIdAsync(It.IsAny<Guid>()),
                Times.Never);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.AtualizarAsync(It.IsAny<ProfissionalSaude>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Profissional_Nao_Existir()
        {
            // Arrange
            var profissionalId = Guid.NewGuid();
            var profissionalRequest = CriarProfissionalSaudeRequestValido();

            var command = new UpdateProfissionalSaudeCommand
            {
                Id = profissionalId,
                Item = profissionalRequest
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _profissionalSaudeRepositoryMock
                .Setup(x => x.ObterPorIdAsync(profissionalId))
                .ReturnsAsync((ProfissionalSaude?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _mediatorHandlerMock.Verify(
                x => x.PublicarNotificacao(It.IsAny<Notificacao>()),
                Times.Once);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.AtualizarAsync(It.IsAny<ProfissionalSaude>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Atualizar_Profissional_Quando_Command_For_Valido()
        {
            // Arrange
            var profissionalId = Guid.NewGuid();

            var profissionalExistente = new ProfissionalSaude(
                "Dra. Ana Souza",
                "Cardiologia",
                "CRM123456");

            var profissionalRequest = CriarProfissionalSaudeRequestValido();

            var profissionalMapeado = new ProfissionalSaude(
                profissionalRequest.nomeCompleto,
                profissionalRequest.especialidade,
                profissionalRequest.numeroRegistro);

            var command = new UpdateProfissionalSaudeCommand
            {
                Id = profissionalId,
                Item = profissionalRequest
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _profissionalSaudeRepositoryMock
                .Setup(x => x.ObterPorIdAsync(profissionalId))
                .ReturnsAsync(profissionalExistente);

            _mapperMock
                .Setup(x => x.Map<ProfissionalSaude>(profissionalRequest))
                .Returns(profissionalMapeado);

            _profissionalSaudeRepositoryMock
                .Setup(x => x.AtualizarAsync(It.IsAny<ProfissionalSaude>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.NomeCompleto.Should().Be(profissionalRequest.nomeCompleto);
            result.Especialidade.Should().Be(profissionalRequest.especialidade);
            result.NumeroRegistro.Should().Be(profissionalRequest.numeroRegistro);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.ObterPorIdAsync(profissionalId),
                Times.Once);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.AtualizarAsync(It.IsAny<ProfissionalSaude>()),
                Times.Once);
        }

        private static CriarProfissionalSaude CriarProfissionalSaudeRequestValido()
        {
            return new CriarProfissionalSaude
            {
                nomeCompleto = "Dra. Maria Oliveira",
                especialidade = "Clínica Geral",
                numeroRegistro = "CRM654321"
            };
        }
    }
}
