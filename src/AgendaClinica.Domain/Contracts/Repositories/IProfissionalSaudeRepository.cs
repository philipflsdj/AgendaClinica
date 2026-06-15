using AgendaClinica.Domain.Entities;

namespace AgendaClinica.Domain.Contracts.Repositories
{
    public interface IProfissionalSaudeRepository
    {
        Task AdicionarAsync(ProfissionalSaude profissional);

        Task<ProfissionalSaude?> ObterPorIdAsync(Guid id);

        Task<ProfissionalSaude?> ObterPorNumeroRegistroAsync(string numeroRegistro);

        Task<IEnumerable<ProfissionalSaude>> ObterTodosAsync();

        Task<bool> NumeroRegistroJaExisteAsync(string numeroRegistro);

        Task AtualizarAsync(ProfissionalSaude profissional);

        Task RemoverAsync(Guid id);
    }
}
