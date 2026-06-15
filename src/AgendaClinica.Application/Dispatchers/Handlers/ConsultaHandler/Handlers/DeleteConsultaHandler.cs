using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers
{
    public sealed class DeleteConsultaHandler : IRequestHandler<DeleteConsultaCommand, bool>
    {
        private readonly IConsultaRepository _consultaRepository;
        public DeleteConsultaHandler(IConsultaRepository consultaRepository)
        {
            _consultaRepository = consultaRepository;
        }

        public async Task<bool> Handle(DeleteConsultaCommand request, CancellationToken cancellationToken)
        {
            var consulta = await _consultaRepository.ObterPorIdAsync(request.Id);
            if (consulta is null)
                return false;

            await _consultaRepository.CancelarAsync(request.Id);

            return true;
        }
    }
}
