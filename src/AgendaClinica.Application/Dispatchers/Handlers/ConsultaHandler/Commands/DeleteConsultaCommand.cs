using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands
{
    public sealed class DeleteConsultaCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
