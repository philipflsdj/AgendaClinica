using AgendaClinica.Application.Configurations.DTOs.Pacientes;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands
{
    public sealed class AddPacienteCommand : IRequest<Paciente>
    {
        public CriarPaciente Item { get; set; }
    }
}
