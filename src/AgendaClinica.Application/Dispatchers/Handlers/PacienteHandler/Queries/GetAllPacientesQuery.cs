using AgendaClinica.Domain.Entities;
using MediatR;


namespace AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Queries
{
    public class GetAllPacientesQuery : IRequest<IEnumerable<Paciente>>
    {
    }
}
