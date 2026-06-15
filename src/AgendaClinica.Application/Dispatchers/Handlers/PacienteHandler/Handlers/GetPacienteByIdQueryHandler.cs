using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using MediatR;


namespace AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Handlers
{
    public class GetPacienteByIdQueryHandler : IRequestHandler<GetPacienteByIdQuery, Paciente?>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMediator _mediator;

        public GetPacienteByIdQueryHandler(IPacienteRepository pacienteRepository, IMediator mediator)
        {
            _pacienteRepository = pacienteRepository;
            _mediator = mediator;
        }

        public async Task<Paciente?> Handle(GetPacienteByIdQuery request, CancellationToken cancellationToken)
        {
            return await _pacienteRepository.ObterPorIdAsync(request.Id);
        }
    }
}
