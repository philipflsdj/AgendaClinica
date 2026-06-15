using AgendaClinica.Application.Configurations.DTOs.Auth;
using AgendaClinica.Domain.SeedWorks;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Commands
{
    public class RegisterUserCommand : IRequest<Result<AuthResponseDto>>
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
