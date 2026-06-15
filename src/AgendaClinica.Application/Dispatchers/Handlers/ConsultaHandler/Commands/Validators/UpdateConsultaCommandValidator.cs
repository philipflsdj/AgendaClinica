using Application;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands.Validators
{
    public sealed class UpdateConsultaCommandValidator : AbstractValidator<UpdateConsultaCommand>
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UpdateConsultaCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Item.ProfissionalId)
                .NotEmpty().WithMessage(_localizer["FiedRequired"]);

            RuleFor(x => x.Item.PacienteId)
                .NotEmpty().WithMessage(_localizer["FiedRequired"]);


            RuleFor(x => x.Item.InicioEm)
                .NotEmpty().WithMessage(_localizer["FiedRequired"]);


        }
    }
}
