using AgendaClinica.Api.Controllers.Base;
using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaClinica.Api.Controllers
{
    [AllowAnonymous]
    public class AuthController : MainController
    {
        public AuthController(INotificador notificador,
                                           IMediatorHandler mediatorHandler,
                                           IMapper mapper,
                                           IMediator mediator) :
            base(notificador, mediatorHandler, mediator, mapper)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CustomResponse(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CustomResponse(result);
        }
    }
}
