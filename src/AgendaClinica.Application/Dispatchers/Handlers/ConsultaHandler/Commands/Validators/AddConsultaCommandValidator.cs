

using Application;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands.Validators
{
    public sealed class AddConsultaCommandValidator : AbstractValidator<AddConsultaCommand>
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AddConsultaCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Consulta.PacienteId)
                .NotEmpty().WithMessage(_localizer["FiedRequired"]);

            RuleFor(x => x.Consulta.ProfissionalId)
                .NotEmpty().WithMessage(_localizer["FiedRequired"]);


            RuleFor(x => x.Consulta.InicioEm)
                .NotEmpty().WithMessage(_localizer["FiedRequired"]);


        }
    }
}
