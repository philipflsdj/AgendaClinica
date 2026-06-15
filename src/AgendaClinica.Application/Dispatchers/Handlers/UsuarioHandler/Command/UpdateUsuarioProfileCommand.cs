using MediatR;
using AgendaClinica.Domain.SeedWorks;
using AgendaClinica.Application.Configurations.DTOs.Usuarios;

namespace AgendaClinica.Application.Dispatchers.Handlers.UsuarioHandler.Command
{
    public class UpdateUsuarioProfileCommand : IRequest<Result<UsuarioDto>>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }
}
