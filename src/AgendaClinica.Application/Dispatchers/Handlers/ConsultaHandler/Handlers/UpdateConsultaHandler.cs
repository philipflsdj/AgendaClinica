using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using Application;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Localization;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers
{
    public sealed class UpdateConsultaHandler : IRequestHandler<UpdateConsultaCommand, Consulta>
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IValidator<UpdateConsultaCommand> _validator;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;


        public UpdateConsultaHandler(IConsultaRepository consultaRepository, 
                                     IValidator<UpdateConsultaCommand> validator, 
                                     IMediatorHandler mediatorHandler,
                                     IStringLocalizer<SharedResource> localizer,
                                     IMapper mapper 
                                     )
        {
            _consultaRepository = consultaRepository;
            _validator = validator;
            _mediatorHandler = mediatorHandler;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Consulta> Handle(UpdateConsultaCommand request, CancellationToken cancellationToken)
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

            var consulta = await _consultaRepository.ObterPorIdAsync(request.Id);
            if (consulta is null)
                return null;

            consulta.Atualizar(_mapper.Map<Consulta>(request.Item));

            await _consultaRepository.AtualizarAsync(_mapper.Map<Consulta>(consulta));

            return _mapper.Map<Consulta>(request.Item);
            
        }
    }
}
