using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries
{
    public sealed class GetAllConsultasQuery : IRequest<IEnumerable<Consulta?>>
    {

    }
}
