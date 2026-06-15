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
    public class AddProfissionalSaudeCommandHandler : IRequestHandler<AddProfissionalSaudeCommand, ProfissionalSaude>
    {
        private readonly IProfissionalSaudeRepository _profissionalSaudeRepository;
        private readonly IValidator<AddProfissionalSaudeCommand> _validator;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;

        public AddProfissionalSaudeCommandHandler(IProfissionalSaudeRepository profissionalSaudeRepository,
                                                  IValidator<AddProfissionalSaudeCommand> validator,
                                                  IMediatorHandler mediatorHandler,
                                                  IStringLocalizer<SharedResource> localizer,
                                                  IMapper mapper
            )
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
            _validator = validator;
            _mediatorHandler = mediatorHandler;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<ProfissionalSaude> Handle(AddProfissionalSaudeCommand request, CancellationToken cancellationToken)
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

            ProfissionalSaude existingPeriodicityType = await _profissionalSaudeRepository.ObterPorNumeroRegistroAsync(request.Item.numeroRegistro);

            if (existingPeriodicityType != null )
            {
                await _mediatorHandler.PublicarNotificacao(new Notificacao(_localizer["NotFound"]));
                return null;
            }
            var profissional = new ProfissionalSaude(_mapper.Map<ProfissionalSaude>( request.Item));



            await _profissionalSaudeRepository.AdicionarAsync(profissional);

            return _mapper.Map<ProfissionalSaude>(request.Item);
            
        }
    }
}
