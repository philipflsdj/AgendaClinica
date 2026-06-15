using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers
{
    public sealed class GetConsultaByIdHandler : IRequestHandler<GetConsultaByIdQuery, Consulta?>
    {
        private readonly IMediator _mediator;
        private readonly IConsultaRepository _consultaRepository;

        public GetConsultaByIdHandler(IMediator mediator, IConsultaRepository consultaRepository)
        {
            _mediator = mediator;
            _consultaRepository = consultaRepository;
        }

        public Task<Consulta?> Handle(GetConsultaByIdQuery request, CancellationToken cancellationToken)
        {
            return _consultaRepository.ObterPorIdAsync(request.Id);
        }
    }
}
