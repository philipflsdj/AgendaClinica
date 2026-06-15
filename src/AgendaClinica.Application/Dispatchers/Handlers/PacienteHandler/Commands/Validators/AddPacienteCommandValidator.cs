using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands.Validators
{
    public class AddPacienteCommandValidator : AbstractValidator<AddPacienteCommand>
    {
        public AddPacienteCommandValidator()
        {
            RuleFor(x => x.Item.nomeCompleto)
                .NotEmpty().WithMessage("O Nome Completo é Obrigatório.")
                .MaximumLength(150).WithMessage("O nome completo deve ter no máximo 150 caracteres.");

            RuleFor(x => x.Item.documento)
                .NotEmpty().WithMessage("O Documento é Obrigatório");
            
            RuleFor(x => x.Item.telefone)
                .NotEmpty().WithMessage("O Telefone é Obrigatório.")
                .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.");

            RuleFor(x => x.Item.email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.")
                .MaximumLength(150).WithMessage("O e-mail deve ter no máximo 150 caracteres.");
        }
    }
}
