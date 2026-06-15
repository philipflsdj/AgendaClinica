using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using Application;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Localization;


namespace AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Handlers
{
    public class AddPacienteCommandHandler : IRequestHandler<AddPacienteCommand, Paciente>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IValidator<AddPacienteCommand> _validator;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;


        public AddPacienteCommandHandler(IPacienteRepository pacienteRepository, 
                                         IValidator<AddPacienteCommand> validator,
                                         IMediatorHandler mediatorHandler,
                                         IStringLocalizer<SharedResource> localizer,
                                         IMapper mapper)
        {
            _pacienteRepository = pacienteRepository;
            _validator = validator;
            _mediatorHandler = mediatorHandler;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Paciente> Handle(AddPacienteCommand request, CancellationToken cancellationToken)
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

            var existente = await _pacienteRepository.DocumentoJaExisteAsync(request.Item.documento);
            if (existente != false)
            {
                await _mediatorHandler.PublicarNotificacao(new Notificacao(_localizer["NotFound"]));
                return null;
            }

            var paciente = new Paciente(_mapper.Map<Paciente>(request.Item));

            if (!paciente.IsValid)
            {
                await _mediatorHandler.PublicarNotificacao(new Notificacao(_localizer["NotFound"]));
                return null;
            }

            await _pacienteRepository.AdicionarAsync(paciente);

            return paciente;
        }
    }
}
