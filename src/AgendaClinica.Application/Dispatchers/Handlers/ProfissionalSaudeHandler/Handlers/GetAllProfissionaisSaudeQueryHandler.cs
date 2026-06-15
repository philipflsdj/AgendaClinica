using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using MediatR;


namespace AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Handlers
{
    public class GetAllProfissionaisSaudeQueryHandler : IRequestHandler<GetAllProfissionaisSaudeQuery, IEnumerable<ProfissionalSaude>>
    {
        private readonly IProfissionalSaudeRepository _profissionalSaudeRepository;

        public GetAllProfissionaisSaudeQueryHandler(IProfissionalSaudeRepository profissionalSaudeRepository)
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
        }

        public async Task<IEnumerable<ProfissionalSaude>> Handle(GetAllProfissionaisSaudeQuery request, CancellationToken cancellationToken)
        {
            return await _profissionalSaudeRepository.ObterTodosAsync();
        }
    }
}
