using AgendaClinica.Application.Configurations.DTOs.Auth;
using AgendaClinica.Application.Configurations.Interfaces;
using AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Commands;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Contracts.Services;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Domain.SeedWorks;
using FluentValidation;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Handler
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<AuthResponseDto>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasherService _passwordService;
        private readonly IValidator<RegisterUserCommand> _validator;
        private readonly IMediatorHandler _mediatorHandler;

        public RegisterUserHandler(
            IUsuarioRepository usuarioRepository,
            ITokenService tokenService,
            IPasswordHasherService passwordService,
            IValidator<RegisterUserCommand> validator,
            IMediatorHandler mediatorHandler)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _validator = validator;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<Result<AuthResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);

            if (!validation.IsValid)
                return Result<AuthResponseDto>.Fail(validation.Errors.Select(x => x.ErrorMessage));

            var usuarioExistente = await _usuarioRepository.ObterPorEmailAsync(request.Email);

            if (usuarioExistente is not null)
                return Result<AuthResponseDto>.Fail("Já existe um usuário com este e-mail.");

            var passwordHash = _passwordService.HashPassword(request.Password);

            var usuario = new Usuario(
                request.Nome,
                request.Email,
                passwordHash,
                "USER"
            );

            if (!usuario.IsValid)
                return Result<AuthResponseDto>.Fail(usuario.Notifications);

            await _usuarioRepository.AdicionarAsync(usuario);

            var token = _tokenService.GenerateToken(usuario);

            return Result<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Email
            }, "Usuário registrado com sucesso.");
        }
    }
}