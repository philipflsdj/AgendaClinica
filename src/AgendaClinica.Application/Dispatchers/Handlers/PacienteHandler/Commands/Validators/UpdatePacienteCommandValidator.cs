using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands.Validators
{
    public class UpdatePacienteCommandValidator : AbstractValidator<UpdatePacienteCommand>
    {
        public UpdatePacienteCommandValidator()
        {
            RuleFor(x => x.Item.nomeCompleto)
                .NotEmpty().WithMessage("O nome completo é obrigatório.")
                .MaximumLength(150).WithMessage("O nome completo deve ter no máximo 150 caracteres.");

            RuleFor(x => x.Item.documento)
                .NotEmpty().WithMessage("O Documento é obrigatório.");


            RuleFor(x => x.Item.telefone)
                .NotEmpty().WithMessage("O telefone é obrigatório.")
                .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.");

            RuleFor(x => x.Item.email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.")
                .MaximumLength(150).WithMessage("O e-mail deve ter no máximo 150 caracteres.");
        }
    }
}
