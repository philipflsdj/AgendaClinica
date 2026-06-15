using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace AgendaClinica.Api.Controllers.Base
{
    [ApiController]
    public class MainController : ControllerBase
    {
        protected INotificador _notificador;
        protected IMediatorHandler _mediatorHandler;
        protected IMediator _mediator;
        protected IMapper _mapper;

        public MainController(INotificador notificador,
                              IMediatorHandler mediatorHandler,
                              IMediator mediator,
                              IMapper mapper)
        {
            _notificador = (Notificador)notificador; 
            _mediatorHandler = mediatorHandler;
            _mediator = mediator;
            _mapper = mapper;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult CustomResponse(object? result = null,
                                              HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (OperacaoValida())
            {
                return new ObjectResult(result)
                {
                    StatusCode = (int)statusCode,
                };
            }

            return NotFound(new
            {
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErrosDeValidacao(modelState).Wait();
            return CustomResponse();
        }

        protected async Task NotificarErrosDeValidacao(ModelStateDictionary modelState)
        {
            foreach (var error in modelState.Values.SelectMany(e => e.Errors))
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                await NotificarErroAsync(errorMsg);
            }
        }

        protected async Task NotificarErroAsync(string mensagem)
        {
            throw new NotImplementedException();
            //await _mediatorHandler.PublicarNotificacao(new Notificacao(mensagem));
        }

        protected async Task Notificar(string mensagem)
        {
            throw new NotImplementedException();
            //await _mediatorHandler.PublicarNotificacao(new Notificacao(mensagem));
        }
    }
}
