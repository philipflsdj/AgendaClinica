using AgendaClinica.Application.Configurations.DTOs.Usuarios;
using AgendaClinica.Application.Dispatchers.Handlers.UsuarioHandler.Command;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.SeedWorks;
using AutoMapper;
using FluentValidation;
using MediatR;


namespace AgendaClinica.Application.Dispatchers.Handlers.UsuarioHandler.Handler
{
    public class UpdateUsuarioProfileHandler : IRequestHandler<UpdateUsuarioProfileCommand, Result<UsuarioDto>>
    {
        private readonly IUsuarioRepository _repository;
        private readonly IValidator<UpdateUsuarioProfileCommand> _validator;
        private readonly IMapper _mapper;

        public UpdateUsuarioProfileHandler(
            IUsuarioRepository repository,
            IValidator<UpdateUsuarioProfileCommand> validator,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Result<UsuarioDto>> Handle(UpdateUsuarioProfileCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);

            if (!validation.IsValid)
                return Result<UsuarioDto>.Fail(validation.Errors.Select(x => x.ErrorMessage));

            var usuario = await _repository.ObterPorIdAsync(request.Id);

            if (usuario is null)
                return Result<UsuarioDto>.Fail("Usuário não encontrado.");

            usuario.AtualizarPerfil(request.Nome);

            if (!usuario.IsValid)
                return Result<UsuarioDto>.Fail(usuario.Notifications);

            await _repository.AtualizarPerfilAsync(usuario);

            return Result<UsuarioDto>.Ok(
                _mapper.Map<UsuarioDto>(usuario),
                "Perfil atualizado com sucesso."
            );
        }
    }
}
