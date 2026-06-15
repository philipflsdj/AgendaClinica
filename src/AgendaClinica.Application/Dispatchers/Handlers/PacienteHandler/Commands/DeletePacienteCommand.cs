using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands
{
    public sealed class DeletePacienteCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
