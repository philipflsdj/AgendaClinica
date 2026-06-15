using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using Application;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Localization;
using AutoMapper;

namespace AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Handlers
{
    public class UpdatePacienteCommandHandler : IRequestHandler<UpdatePacienteCommand, Paciente>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IValidator<UpdatePacienteCommand> _validator;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _Mapper;

        public UpdatePacienteCommandHandler(IPacienteRepository pacienteRepository, 
                                            IValidator<UpdatePacienteCommand> validator,
                                            IMediatorHandler mediatorHandler,
                                            IStringLocalizer<SharedResource> localizer,
                                            IMapper mapper)
        {
            _pacienteRepository = pacienteRepository;
            _validator = validator;
            _mediatorHandler = mediatorHandler;
            _localizer = localizer;
            _Mapper = mapper;
        }

        public async Task<Paciente> Handle(UpdatePacienteCommand request, CancellationToken cancellationToken)
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

            var paciente = await _pacienteRepository.ObterPorIdAsync(request.Id);

            if (paciente is null)
            {
                await _mediatorHandler.PublicarNotificacao(new Notificacao("Paciente Não Encontrado!"));
                return null;
            }


            paciente.Atualizar(_Mapper.Map<Paciente>(request.Item));

            if (!paciente.IsValid)
                return null;

            await _pacienteRepository.AtualizarAsync(paciente);

            return _Mapper.Map<Paciente>(request.Item);
        }
    }
}
