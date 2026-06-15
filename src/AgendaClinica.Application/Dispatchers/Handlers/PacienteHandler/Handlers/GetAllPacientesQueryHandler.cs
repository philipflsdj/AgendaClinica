using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Handlers
{
    public class GetAllPacientesQueryHandler : IRequestHandler<GetAllPacientesQuery, IEnumerable<Paciente>>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMediator _mediator;

        public GetAllPacientesQueryHandler(IPacienteRepository pacienteRepository, IMediator mediator)
        {
            _pacienteRepository = pacienteRepository;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Paciente>> Handle(GetAllPacientesQuery request, CancellationToken cancellationToken)
        {
            return await _pacienteRepository.ObterTodosAsync();
        }
    }
}
