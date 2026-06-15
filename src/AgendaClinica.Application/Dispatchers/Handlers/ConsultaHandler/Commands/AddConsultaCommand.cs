using AgendaClinica.Application.Configurations.DTOs.Consultas;
using AgendaClinica.Domain.Entities;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands
{
    public sealed class AddConsultaCommand : IRequest<Consulta>
    {
        public CriarConsultaRequest Consulta { get; set; } = null!;
    }
}
