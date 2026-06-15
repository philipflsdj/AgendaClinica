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
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers
{
    public sealed class AddConsultaHandler : IRequestHandler<AddConsultaCommand, Consulta>
    {
        private readonly IValidator<AddConsultaCommand> _validator;
        private readonly IConsultaRepository _consultaRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;



        public AddConsultaHandler(IMediatorHandler mediatorHandler,
                                  IValidator<AddConsultaCommand> validator,
                                  IStringLocalizer<SharedResource> localizer,
                                  IConsultaRepository consultaRepository,
                                  IMapper mapper) 
        {
            _mediatorHandler = mediatorHandler;
            _validator = validator;
            _localizer = localizer;
            _consultaRepository = consultaRepository;
            _mapper = mapper;
        }

        public async Task<Consulta> Handle(AddConsultaCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return null;

            ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    await _mediatorHandler.PublicarNotificacao(new Notificacao(_localizer[error.ErrorMessage] ?? error.ErrorMessage));
                }
                return null;
            }


            bool consultaExistente = await _consultaRepository
                .PacientePossuiConsultaComProfissionalNoDiaAsync(
                request.Consulta.PacienteId, 
                request.Consulta.ProfissionalId, 
                request.Consulta.InicioEm);

            if (consultaExistente)
            {
                await _mediatorHandler.PublicarNotificacao(new Notificacao(_localizer["NotFound"]));
                return null;
            }
            try
            {
                await _consultaRepository.AdicionarAsync(_mapper.Map<Consulta>(request.Consulta));

            }
            catch (Exception Ex)
            {

                throw;
            }
            return _mapper.Map<Consulta>(request.Consulta);



        }
    }
}
