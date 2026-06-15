using AgendaClinica.Application.Configurations.DTOs.Consultas;
using AgendaClinica.Domain.Entities;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands
{
    public sealed class UpdateConsultaCommand : IRequest<Consulta>
    {
        public CriarConsultaRequest Item { get; set; }
        public Guid Id { get; set; }
    }
}
