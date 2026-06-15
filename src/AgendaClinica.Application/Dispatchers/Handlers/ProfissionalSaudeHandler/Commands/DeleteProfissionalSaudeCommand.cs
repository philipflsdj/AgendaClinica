using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Commands
{
    public sealed class DeleteProfissionalSaudeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
