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
    public class AddProfissionalSaudeCommandHandlerTests
    {
        private readonly Mock<IProfissionalSaudeRepository> _profissionalSaudeRepositoryMock;
        private readonly Mock<IValidator<AddProfissionalSaudeCommand>> _validatorMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly AddProfissionalSaudeCommandHandler _handler;

        public AddProfissionalSaudeCommandHandlerTests()
        {
            _profissionalSaudeRepositoryMock = new Mock<IProfissionalSaudeRepository>();
            _validatorMock = new Mock<IValidator<AddProfissionalSaudeCommand>>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _localizerMock = new Mock<IStringLocalizer<SharedResource>>();
            _mapperMock = new Mock<IMapper>();

            _handler = new AddProfissionalSaudeCommandHandler(
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

            var command = new AddProfissionalSaudeCommand
            {
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
                x => x.ObterPorNumeroRegistroAsync(It.IsAny<string>()),
                Times.Never);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<ProfissionalSaude>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_NumeroRegistro_Ja_Existir()
        {
            // Arrange
            var profissionalRequest = CriarProfissionalSaudeRequestValido();

            var command = new AddProfissionalSaudeCommand
            {
                Item = profissionalRequest
            };

            var profissionalExistente = new ProfissionalSaude(
                "Dra. Ana Existente",
                "Cardiologia",
                profissionalRequest.numeroRegistro);

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _profissionalSaudeRepositoryMock
                .Setup(x => x.ObterPorNumeroRegistroAsync(profissionalRequest.numeroRegistro))
                .ReturnsAsync(profissionalExistente);

            _localizerMock
                .Setup(x => x["NotFound"])
                .Returns(new LocalizedString("NotFound", "NotFound"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _mediatorHandlerMock.Verify(
                x => x.PublicarNotificacao(It.IsAny<Notificacao>()),
                Times.Once);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.ObterPorNumeroRegistroAsync(profissionalRequest.numeroRegistro),
                Times.Once);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<ProfissionalSaude>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Adicionar_Profissional_Quando_Command_For_Valido()
        {
            // Arrange
            var profissionalRequest = CriarProfissionalSaudeRequestValido();

            var profissionalMapeado = new ProfissionalSaude(
                profissionalRequest.nomeCompleto,
                profissionalRequest.especialidade,
                profissionalRequest.numeroRegistro);

            var command = new AddProfissionalSaudeCommand
            {
                Item = profissionalRequest
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _profissionalSaudeRepositoryMock
                .Setup(x => x.ObterPorNumeroRegistroAsync(profissionalRequest.numeroRegistro))
                .ReturnsAsync((ProfissionalSaude?)null);

            _mapperMock
                .Setup(x => x.Map<ProfissionalSaude>(profissionalRequest))
                .Returns(profissionalMapeado);

            _profissionalSaudeRepositoryMock
                .Setup(x => x.AdicionarAsync(It.IsAny<ProfissionalSaude>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.NomeCompleto.Should().Be(profissionalRequest.nomeCompleto);
            result.Especialidade.Should().Be(profissionalRequest.especialidade);
            result.NumeroRegistro.Should().Be(profissionalRequest.numeroRegistro);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.ObterPorNumeroRegistroAsync(profissionalRequest.numeroRegistro),
                Times.Once);

            _profissionalSaudeRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<ProfissionalSaude>()),
                Times.Once);
        }

        private static CriarProfissionalSaude CriarProfissionalSaudeRequestValido()
        {
            return new CriarProfissionalSaude
            {
                nomeCompleto = "Dra. Ana Souza",
                especialidade = "Cardiologia",
                numeroRegistro = "CRM123456"
            };
        }
    }
}
