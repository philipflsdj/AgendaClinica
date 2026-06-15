using AgendaClinica.Domain.Entities;

namespace AgendaClinica.Domain.Contracts.Repositories
{
    public interface IConsultaRepository
    {
        Task<Consulta> AdicionarAsync(Consulta consulta);

        Task<Consulta?> ObterPorIdAsync(Guid id);

        Task<IEnumerable<Consulta>> ObterTodasAsync();

        Task<IEnumerable<Consulta>> ObterPorPacienteAsync(Guid pacienteId);

        Task<IEnumerable<Consulta>> ObterPorProfissionalAsync(Guid profissionalId);

        Task<IEnumerable<Consulta>> ObterAgendaProfissionalAsync(
            Guid profissionalId,
            DateTime data);

        Task<bool> ProfissionalPossuiConsultaNoHorarioAsync(
            Guid profissionalId,
            DateTime inicioEm);

        Task<bool> PacientePossuiConsultaComProfissionalNoDiaAsync(
            Guid pacienteId,
            Guid profissionalId,
            DateTime data);

        Task AtualizarAsync(Consulta consulta);

        Task CancelarAsync(Guid id);
    }
}
