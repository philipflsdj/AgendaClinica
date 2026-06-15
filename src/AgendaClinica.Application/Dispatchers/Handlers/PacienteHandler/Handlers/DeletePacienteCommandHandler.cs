using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Handlers
{
    public class DeletePacienteCommandHandler : IRequestHandler<DeletePacienteCommand, bool>
    {
        private readonly IPacienteRepository _pacienteRepository;
        
        public DeletePacienteCommandHandler(IPacienteRepository pacienteRepository, IMediatorHandler mediatorHandler)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async Task<bool> Handle(DeletePacienteCommand request, CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.ObterPorIdAsync(request.Id);
            if (paciente is null)
            {
                return false;
            }

            await _pacienteRepository.RemoverAsync(request.Id);

            return true;
        }
    }
}
