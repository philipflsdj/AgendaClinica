using AgendaClinica.Application.Configurations.AutoMapper;
using AgendaClinica.Application.Configurations.DTOs.Auth;
using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Configurations.Notificacoes;
using AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Commands.Validators;
using AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Handler;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Commands.Validators;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Handlers;
using AgendaClinica.Application.Dispatchers.Handlers.ConsultaHandler.Queries;
using AgendaClinica.Application.Dispatchers.Handlers.Mediatr;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Commands.Validators;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Handlers;
using AgendaClinica.Application.Dispatchers.Handlers.PacienteHandler.Queries;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Commands;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Commands.Validators;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Handlers;
using AgendaClinica.Application.Dispatchers.Handlers.ProfissionalSaudeHandler.Queries;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Contracts.Services;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using AgendaClinica.Infrastructure.Data;
using AgendaClinica.Infrastructure.Repositories;
using AgendaClinica.Infrastructure.Services;
using FluentValidation;
using MediatR;

namespace AgendaClinica.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization();
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

            // Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Notifications
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<INotificationHandler<Notificacao>, Notificador>();


            services.AddMediatR(typeof(GetAllConsultasHandler).Assembly);
            services.AddValidatorsFromAssembly(typeof(GetAllConsultasHandler).Assembly);


            services.AddValidatorsFromAssembly(typeof(DependencyInjectionConfig).Assembly);

            //Repositories
            services.AddScoped<IConsultaRepository, ConsultaRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IProfissionalSaudeRepository, ProfissionalSaudeRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<ITokenService, TokenService>();

            //AutoMapper
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile).Assembly);

            //Login e Auth 
            services.AddScoped<IRequestHandler<LoginCommand, Result<AuthResponseDto>>, LoginHandler>();
            services.AddScoped<IRequestHandler<RegisterUserCommand, Result<AuthResponseDto>>, RegisterUserHandler>();
            services.AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();
            services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();

            // Consulta Handlers
            services.AddScoped<IRequestHandler<GetAllConsultasQuery, IEnumerable<Consulta>?>, GetAllConsultasHandler>();
            services.AddScoped<IRequestHandler<GetConsultaByIdQuery, Consulta?>, GetConsultaByIdHandler>();
            services.AddScoped<IRequestHandler<AddConsultaCommand, Consulta?>, AddConsultaHandler>();
            services.AddScoped<IRequestHandler<DeleteConsultaCommand, bool>, DeleteConsultaHandler>();
            services.AddScoped<IRequestHandler<UpdateConsultaCommand, Consulta?>, UpdateConsultaHandler>();

            services.AddScoped<IValidator<AddConsultaCommand>, AddConsultaCommandValidator>();
            services.AddScoped<IValidator<UpdateConsultaCommand>, UpdateConsultaCommandValidator>();

            // Paciente Handlers
            services.AddScoped<IRequestHandler<GetAllPacientesQuery, IEnumerable<Paciente>?>, GetAllPacientesQueryHandler>();
            services.AddScoped<IRequestHandler<GetPacienteByIdQuery, Paciente?>, GetPacienteByIdQueryHandler>();
            services.AddScoped<IRequestHandler<AddPacienteCommand, Paciente?>, AddPacienteCommandHandler>();
            services.AddScoped<IRequestHandler<DeletePacienteCommand, bool>, DeletePacienteCommandHandler>();
            services.AddScoped<IRequestHandler<UpdatePacienteCommand, Paciente?>, UpdatePacienteCommandHandler>();

            services.AddScoped<IValidator<AddPacienteCommand>, AddPacienteCommandValidator>();
            services.AddScoped<IValidator<UpdatePacienteCommand>, UpdatePacienteCommandValidator>();

            // ProfissionalSaude Handlers
            services.AddScoped<IRequestHandler<GetAllProfissionaisSaudeQuery, IEnumerable<ProfissionalSaude>?>, GetAllProfissionaisSaudeQueryHandler>();
            services.AddScoped<IRequestHandler<GetProfissionalSaudeByIdQuery, ProfissionalSaude?>, GetProfissionalSaudeByIdQueryHandler>();
            services.AddScoped<IRequestHandler<AddProfissionalSaudeCommand, ProfissionalSaude?>, AddProfissionalSaudeCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteProfissionalSaudeCommand, bool>, DeleteProfissionalSaudeCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateProfissionalSaudeCommand, ProfissionalSaude?>, UpdateProfissionalSaudeCommandHandler>();

            services.AddScoped<IValidator<AddProfissionalSaudeCommand>, AddProfissionalSaudeCommandValidator>();
            services.AddScoped<IValidator<UpdateProfissionalSaudeCommand>, UpdateProfissionalSaudeCommandValidator>();



            return services;

        }
    }
}
