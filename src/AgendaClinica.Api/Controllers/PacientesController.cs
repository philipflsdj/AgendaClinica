using AgendaClinica.Api.Controllers.Base;
using AgendaClinica.Application.Configurations.DTOs.Consultas;
using AgendaClinica.Application.Configurations.DTOs.Pacientes;
using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Queries;
using AgendaClinica.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AgendaClinica.Api.Controllers
{
    [Route("api/pacientes")]
    [Authorize]
    public class PacientesController : MainController
    {

        public PacientesController(INotificador notificador,
                                           IMediatorHandler mediatorHandler,
                                           IMapper mapper,
                                           IMediator mediator) :
            base(notificador, mediatorHandler, mediator, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var consultas = await _mediator.Send(new GetAllPacientesQuery());

            return CustomResponse(consultas);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var consulta = await _mediator.Send(new GetPacienteByIdQuery { Id = id });
            if (consulta is null)
                return CustomResponse(null, HttpStatusCode.NotFound);
            return CustomResponse(consulta);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CriarPaciente paciente)
        {
            if (!ModelState.IsValid)
                return CustomResponse(null, HttpStatusCode.NotFound);

            var result = await _mediator.Send(new AddPacienteCommand { Item = paciente });

            return CustomResponse(result, HttpStatusCode.Created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, CriarPaciente Item)
        {
            if (!ModelState.IsValid)
                return CustomResponse(null, HttpStatusCode.NotFound);
            var result = await _mediator.Send(new UpdatePacienteCommand { Item = Item, Id = id });
            return CustomResponse(result, HttpStatusCode.OK);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var removido = await _mediator.Send(new DeletePacienteCommand { Id = id });
            return CustomResponse(HttpStatusCode.OK);
        }
    }
}