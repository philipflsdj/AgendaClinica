

using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers
{
    public class GetAgendaProfissionalHandler : IRequestHandler<GetAgendaProfissionalQuery, Result<List<Consulta>>>
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IMediator _mediator;
        public GetAgendaProfissionalHandler(IConsultaRepository consultaRepository, IMediator mediator)
        {
            _consultaRepository = consultaRepository;
            _mediator = mediator;
        }
        public async Task<Result<List<Consulta>>> Handle(GetAgendaProfissionalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var consultas = await _consultaRepository.ObterAgendaProfissionalAsync(request.ProfissionalId, request.Data);
                return Result<List<Consulta>>.Ok(consultas.ToList(), "Agenda do profissional obtida com sucesso.");
            }
            catch (Exception ex)
            {
                return Result<List<Consulta>>.Fail($"Erro ao obter agenda do profissional: {ex.Message}");
            }
        }
    }
}
