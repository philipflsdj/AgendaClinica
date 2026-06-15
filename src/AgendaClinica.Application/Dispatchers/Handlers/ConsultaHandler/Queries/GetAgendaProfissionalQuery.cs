using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries
{
    public sealed class GetAgendaProfissionalQuery : IRequest<Result<List<Consulta?>>>
    {
        public Guid ProfissionalId { get; set; }
        public DateTime Data { get; set; }
    }
}
