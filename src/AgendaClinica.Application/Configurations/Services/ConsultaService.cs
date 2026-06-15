using AgendaClinica.Application.Configurations.DTOs.Consultas;
using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;


namespace AgendaClinica.Application.Configurations.Services
{
    public class ConsultaService
    {
        private readonly IConsultaRepository _consultaRepository;

        public ConsultaService(IConsultaRepository consultaRepository)
        {
            _consultaRepository = consultaRepository;
        }

        public async Task<ConsultaResponse> AgendarAsync(CriarConsultaRequest request)
        {
            var consulta = new Consulta(
                request.PacienteId,
                request.ProfissionalId,
                request.InicioEm);

            var profissionalIndisponivel =
                await _consultaRepository.ProfissionalPossuiConsultaNoHorarioAsync(
                    request.ProfissionalId,
                    request.InicioEm);

            if (profissionalIndisponivel)
                throw new InvalidOperationException("O profissional já possui uma consulta neste horário.");

            var pacienteJaAgendado =
                await _consultaRepository.PacientePossuiConsultaComProfissionalNoDiaAsync(
                    request.PacienteId,
                    request.ProfissionalId,
                    request.InicioEm.Date);

            if (pacienteJaAgendado)
                throw new InvalidOperationException("O paciente já possui uma consulta com este profissional nesta data.");

            await _consultaRepository.AdicionarAsync(consulta);

            return new ConsultaResponse
            {
                Id = consulta.Id,
                PacienteId = consulta.PacienteId,
                ProfissionalId = consulta.ProfissionalId,
                InicioEm = consulta.InicioEm
            };
        }

        public async Task<IEnumerable<Consulta>> ObterAgendaProfissionalAsync(
            Guid profissionalId,
            DateTime data)
        {
            return await _consultaRepository.ObterAgendaProfissionalAsync(
                profissionalId,
                data);
        }
    }
}
