using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Application.Dispatchers.Handlers.UsuarioHandler.Command.Validators
{
    public class UpdateUsuarioProfileCommandValidator : AbstractValidator<UpdateUsuarioProfileCommand>
    {
        public UpdateUsuarioProfileCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(150);
        }
    }
}
