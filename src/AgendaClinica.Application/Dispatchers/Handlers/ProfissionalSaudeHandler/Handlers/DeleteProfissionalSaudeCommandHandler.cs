using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Commands;
using AgendaClinica.Domain.Contracts.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Handlers
{
    public class DeleteProfissionalSaudeCommandHandler : IRequestHandler<DeleteProfissionalSaudeCommand, bool>
    {
        private readonly IProfissionalSaudeRepository _profissionalSaudeRepository;

        public DeleteProfissionalSaudeCommandHandler(IProfissionalSaudeRepository profissionalSaudeRepository)
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
        }

        public async Task<bool> Handle(DeleteProfissionalSaudeCommand request, CancellationToken cancellationToken)
        {
            var profissionalSaude = await _profissionalSaudeRepository.ObterPorIdAsync(request.Id);

            if (profissionalSaude is null)
                return false;

            await _profissionalSaudeRepository.RemoverAsync(request.Id);

            return true;
        }
    }
}
