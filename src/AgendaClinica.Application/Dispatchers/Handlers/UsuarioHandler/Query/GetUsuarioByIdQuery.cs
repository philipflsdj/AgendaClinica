using AgendaClinica.Application.Configurations.DTOs.Usuarios;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.UsuarioHandler.Query
{
    public class GetUsuarioByIdQuery : IRequest<UsuarioDto?>
    {
        public Guid Id { get; set; }
    }
}
