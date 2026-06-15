using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Handlers
{
    public class GetProfissionalSaudeByIdQueryHandler : IRequestHandler<GetProfissionalSaudeByIdQuery, ProfissionalSaude?>
    {
        private readonly IProfissionalSaudeRepository _profissionalSaudeRepository;

        public GetProfissionalSaudeByIdQueryHandler(IProfissionalSaudeRepository profissionalSaudeRepository)
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
        }

        public async Task<ProfissionalSaude?> Handle(GetProfissionalSaudeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _profissionalSaudeRepository.ObterPorIdAsync(request.Id);
        }
    }
}
