using AgendaClinica.Api.Controllers.Base;
using AgendaClinica.Api.Extension;
using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.UsuarioHandler.Command;
using AgendaClinica.Application.Dispatchers.Handlers.UsuarioHandler.Query;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaClinica.Api.Controllers
{
    [Authorize]
    public class UsuariosController : MainController
    {
        public UsuariosController(INotificador notificador,
                                           IMediatorHandler mediatorHandler,
                                           IMapper mapper,
                                           IMediator mediator) :
            base(notificador, mediatorHandler, mediator, mapper)
        {
        }
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
                return Unauthorized();

            var result = await _mediator.Send(new GetUsuarioByIdQuery { Id = userId.Value });

            if (result is null)
                return CustomResponse("Usuário não encontrado.");

            return Ok(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] UpdateUsuarioProfileCommand command, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
                return Unauthorized();

            command.Id = userId.Value;

            var result = await _mediator.Send(command, cancellationToken);
            return CustomResponse(result);
        }
    }
}
