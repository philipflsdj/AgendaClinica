using AgendaClinica.Application.Configurations.DTOs.ProfissionaisSaude;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Commands
{
    public sealed class AddProfissionalSaudeCommand : IRequest<ProfissionalSaude>
    {
        public CriarProfissionalSaude Item { get; set; }

    }
}
