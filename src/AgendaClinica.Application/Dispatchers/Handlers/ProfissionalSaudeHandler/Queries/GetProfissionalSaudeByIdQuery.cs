using AgendaClinica.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Queries
{
    public class GetProfissionalSaudeByIdQuery : IRequest<ProfissionalSaude?>
    {
        public Guid Id { get; set; }
    }
}
