using AgendaClinica.Application.Configurations.DTOs.Auth;
using AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Commands;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Contracts.Services;
using AgendaClinica.Domain.SeedWorks;
using FluentValidation;
using MediatR;

namespace AgendaClinica.Application.Dispatchers.Handlers.AuthHandler.Handler
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IValidator<LoginCommand> _validator;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasherService _passwordService;

        public LoginHandler(
            IUsuarioRepository usuarioRepository,
            IValidator<LoginCommand> validator,
            ITokenService tokenService,
            IPasswordHasherService passwordService)
        {
            _usuarioRepository = usuarioRepository;
            _validator = validator;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return Result<AuthResponseDto>.Fail(validation.Errors.Select(x => x.ErrorMessage));

            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email);

            if (usuario is null)
                return Result<AuthResponseDto>.Fail("Usuário ou senha inválidos.");

            var senhaValida = _passwordService.VerifyPassword(
                request.Password,
                usuario.SenhaHash);

            if (!senhaValida)
                return Result<AuthResponseDto>.Fail("Usuário ou senha inválidos.");

            var token = _tokenService.GenerateToken(usuario);

            return Result<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Email            
            });
        }
    }
}
