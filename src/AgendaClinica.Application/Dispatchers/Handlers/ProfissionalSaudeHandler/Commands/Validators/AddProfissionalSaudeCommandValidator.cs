using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Commands.Validators
{
    public class AddProfissionalSaudeCommandValidator : AbstractValidator<AddProfissionalSaudeCommand>
    {
        public AddProfissionalSaudeCommandValidator()
        {
            RuleFor(x => x.Item.nomeCompleto)
                .NotEmpty().WithMessage("O nome completo é obrigatório.")
                .MaximumLength(150).WithMessage("O nome completo deve ter no máximo 150 caracteres.");

            RuleFor(x => x.Item.numeroRegistro)
                            .NotEmpty().WithMessage("O Numero de Registro é obrigatória.")
                            .MaximumLength(100).WithMessage("A especialidade deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Item.especialidade)
                .NotEmpty().WithMessage("A especialidade é obrigatória.")
                .MaximumLength(100).WithMessage("A especialidade deve ter no máximo 100 caracteres.");

        }
    }
}
