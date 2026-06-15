using AgendaClinica.Api.Controllers.Base;
using AgendaClinica.Application.Configurations.DTOs.ProfissionaisSaude;
using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Queries;
using AgendaClinica.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AgendaClinica.Api.Controllers
{
    [Route("api/profissionais-saude")]
    [Authorize]
    public class ProfissionaisSaudeController : MainController
    {
        public ProfissionaisSaudeController(INotificador notificador,
                                           IMediatorHandler mediatorHandler,
                                           IMapper mapper,
                                           IMediator mediator) :
            base(notificador, mediatorHandler, mediator, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var consultas = await _mediator.Send(new GetAllProfissionaisSaudeQuery());

            return CustomResponse(consultas);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var consulta = await _mediator.Send(new GetProfissionalSaudeByIdQuery { Id = id });
            if (consulta is null)
                return CustomResponse(null, HttpStatusCode.NotFound);
            return CustomResponse(consulta);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CriarProfissionalSaude  profissionalSaude)
        {
            if (!ModelState.IsValid)
                return CustomResponse(null, HttpStatusCode.NotFound);

            var result = await _mediator.Send(new AddProfissionalSaudeCommand { Item = profissionalSaude });

            return CustomResponse(result, HttpStatusCode.Created);


        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, CriarProfissionalSaude Item)
        {
            if (!ModelState.IsValid)
                return CustomResponse(null, HttpStatusCode.NotFound);
            var result = await _mediator.Send(new UpdateProfissionalSaudeCommand { Item = Item, Id = id });
            return CustomResponse(result, HttpStatusCode.OK);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var removido = await _mediator.Send(new DeleteProfissionalSaudeCommand { Id = id });
            return CustomResponse(HttpStatusCode.OK);
        }

    }
}