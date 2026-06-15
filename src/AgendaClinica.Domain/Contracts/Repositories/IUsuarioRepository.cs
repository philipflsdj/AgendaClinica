using AgendaClinica.Domain.Entities;


namespace AgendaClinica.Domain.Contracts.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorIdAsync(Guid id);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<IEnumerable<Usuario>> ObterTodosAsync();
        Task AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task AtualizarPerfilAsync(Usuario usuario);
        Task RemoverAsync(Guid id);
    }
}
