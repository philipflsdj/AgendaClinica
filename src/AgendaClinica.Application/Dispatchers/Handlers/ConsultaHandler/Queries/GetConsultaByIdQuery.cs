using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries
{
    public sealed class GetConsultaByIdQuery : IRequest<Consulta?>
    {
        public Guid Id { get; set; }
    }
}
