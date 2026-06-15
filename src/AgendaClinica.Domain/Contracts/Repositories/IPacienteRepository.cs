using AgendaClinica.Domain.Entities;

namespace AgendaClinica.Domain.Contracts.Repositories
{
    public interface IPacienteRepository
    {
        Task AdicionarAsync(Paciente paciente);

        Task<Paciente?> ObterPorIdAsync(Guid id);

        Task<Paciente?> ObterPorDocumentoAsync(string documento);

        Task<IEnumerable<Paciente>> ObterTodosAsync();

        Task<bool> DocumentoJaExisteAsync(string documento);

        Task<bool> EmailJaExisteAsync(string email);

        Task AtualizarAsync(Paciente paciente);

        Task RemoverAsync(Guid id);
    }
}
