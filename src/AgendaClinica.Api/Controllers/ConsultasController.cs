using AgendaClinica.Api.Controllers.Base;
using AgendaClinica.Application.Configurations.DTOs.Consultas;
using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AgendaClinica.Api.Controllers
{
    [Route("api/consultas")]
    [Authorize]

    public class ConsultasController : MainController
    {

        public ConsultasController(INotificador notificador,
                                           IMediatorHandler mediatorHandler,
                                           IMapper mapper,
                                           IMediator mediator) :
            base(notificador, mediatorHandler, mediator, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var consultas = await _mediator.Send(new GetAllConsultasQuery());

            return CustomResponse(consultas);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var consulta = await _mediator.Send(new GetConsultaByIdQuery { Id = id });
            if (consulta is null)
                return CustomResponse(null, HttpStatusCode.NotFound);
            return CustomResponse(consulta);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CriarConsultaRequest consulta)
        {
            if (!ModelState.IsValid)
                return CustomResponse(null, HttpStatusCode.NotFound);

            var result = await _mediator.Send(new AddConsultaCommand {Consulta = consulta });

            return CustomResponse(result, HttpStatusCode.Created);


        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, CriarConsultaRequest Item)
        {
            if (!ModelState.IsValid)
                return CustomResponse(null, HttpStatusCode.NotFound);            
            var result = await _mediator.Send(new UpdateConsultaCommand { Item = Item, Id = id });
            return CustomResponse(result, HttpStatusCode.OK);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var removido = await _mediator.Send(new DeleteConsultaCommand { Id = id });
            return CustomResponse(HttpStatusCode.OK);
        }
    }        
}