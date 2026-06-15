

using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers
{
    public sealed class GetAllConsultasHandler : IRequestHandler<GetAllConsultasQuery, IEnumerable<Consulta?>>
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IMediator _mediator;

        public GetAllConsultasHandler(IConsultaRepository consultaRepository, IMediator mediator)
        {
            _consultaRepository = consultaRepository;
            _mediator = mediator;
        }
        public async Task<IEnumerable<Consulta?>> Handle(GetAllConsultasQuery request, CancellationToken cancellationToken)
        {
            var consultas = await _consultaRepository.ObterTodasAsync();
            return consultas.ToList();
        }
    }
}
