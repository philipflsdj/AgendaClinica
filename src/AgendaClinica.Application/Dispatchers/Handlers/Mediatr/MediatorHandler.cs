using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Application.Dispatchers.Handlers.Mediatr
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
            //_eventSourcingRepository = eventSourcingRepository;
        }

        public async Task PublicarNotificacao<T>(T notificacao) where T : Notificacao
        {
            await _mediator.Publish(notificacao);
        }

    }
}
