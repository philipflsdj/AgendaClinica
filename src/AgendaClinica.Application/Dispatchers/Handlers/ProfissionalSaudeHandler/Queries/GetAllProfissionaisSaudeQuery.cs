using AgendaClinica.Domain.Entities;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Queries
{
    public class GetAllProfissionaisSaudeQuery : IRequest<IEnumerable<ProfissionalSaude>>
    {
    }
}
