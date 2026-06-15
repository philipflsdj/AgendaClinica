using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Commands;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using Application;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Localization;
using AutoMapper;

namespace AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Handlers
{
    public class UpdateProfissionalSaudeCommandHandler : IRequestHandler<UpdateProfissionalSaudeCommand, ProfissionalSaude>
    {
        private readonly IProfissionalSaudeRepository _profissionalSaudeRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IValidator<UpdateProfissionalSaudeCommand> _validator;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public UpdateProfissionalSaudeCommandHandler(IProfissionalSaudeRepository profissionalSaudeRepository,
            IValidator<UpdateProfissionalSaudeCommand> validator,
                                     IMediatorHandler mediatorHandler,
                                     IStringLocalizer<SharedResource> localizer,
                                     IMapper mapper
            )
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
            _mediatorHandler = mediatorHandler;
            _validator = validator;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<ProfissionalSaude> Handle(UpdateProfissionalSaudeCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    await _mediatorHandler.PublicarNotificacao(new Notificacao(_localizer[error.ErrorMessage] ?? error.ErrorMessage));
                }
                return null;
            }

            var profissionalSaude = await _profissionalSaudeRepository.ObterPorIdAsync(request.Id);
            if (profissionalSaude is null)
            {
                await _mediatorHandler.PublicarNotificacao(new Notificacao("Profissional Não Existe"));

                return null;             
            }

            profissionalSaude.Atualizar(_mapper.Map<ProfissionalSaude>(request.Item));

            if (!profissionalSaude.IsValid)
                await _mediatorHandler.PublicarNotificacao(new Notificacao("Profissional Não Existe"));


            await _profissionalSaudeRepository.AtualizarAsync(profissionalSaude);

            return _mapper.Map<ProfissionalSaude>(request.Item);

        }
    }
}
