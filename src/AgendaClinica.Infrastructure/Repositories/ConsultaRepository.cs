using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Infrastructure.Data;
using Dapper;

namespace AgendaClinica.Infrastructure.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ConsultaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Consulta> AdicionarAsync(Consulta consulta)
        {
            const string sql = """
        INSERT INTO appointments
        (
            id,
            patient_id,
            professional_id,
            start_at,
            end_at,
            status,
            created_at
        )
        VALUES
        (
            @Id,
            @PacienteId,
            @ProfissionalId,
            @InicioEm,
            @FimEm,
            @Status,
            @CreatedOn
        );
    """;

            using var connection = _connectionFactory.CreateConnection();
            try
            {
                await connection.ExecuteAsync(sql, new
                {
                    consulta.Id,
                    consulta.PacienteId,
                    consulta.ProfissionalId,
                    consulta.InicioEm,
                    consulta.FimEm,
                    consulta.Status,
                    CreatedOn = consulta.CreatedOn ?? DateTime.UtcNow
                });

            }
            catch (Exception EX)
            {

                throw;
            }

            return consulta;
        }

        public async Task<Consulta?> ObterPorIdAsync(Guid id)
        {
            const string sql = """
                SELECT
                    id AS Id,
                    patient_id AS PacienteId,
                    professional_id AS ProfissionalId,
                    start_at AS InicioEm,
                    end_at AS FimEm,
                    status AS Status,
                    created_at AS CreatedOn,
                    updated_at AS UpdatedOn
                FROM appointments
                WHERE id = @Id;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Consulta>(sql, new
            {
                Id = id
            });
        }

        public async Task<IEnumerable<Consulta>> ObterTodasAsync()
        {
            const string sql = """
                SELECT
                    id AS Id,
                    patient_id AS PacienteId,
                    professional_id AS ProfissionalId,
                    start_at AS InicioEm,
                    end_at AS FimEm,
                    status AS Status,
                    created_at AS CreatedOn,
                    updated_at AS UpdatedOn
                FROM appointments
                WHERE status <> 'CANCELED' 
                ORDER BY start_at DESC;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Consulta>(sql);
        }

        public async Task<IEnumerable<Consulta>> ObterPorPacienteAsync(Guid pacienteId)
        {
            const string sql = """
                SELECT
                    id AS Id,
                    patient_id AS PacienteId,
                    professional_id AS ProfissionalId,
                    start_at AS InicioEm,
                    end_at AS FimEm,
                    status AS Status,
                    created_at AS CreatedOn,
                    updated_at AS UpdatedOn
                FROM appointments
                WHERE patient_id = @PacienteId
                ORDER BY start_at DESC;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Consulta>(sql, new
            {
                PacienteId = pacienteId
            });
        }

        public async Task<IEnumerable<Consulta>> ObterPorProfissionalAsync(Guid profissionalId)
        {
            const string sql = """
                SELECT
                    id AS Id,
                    patient_id AS PacienteId,
                    professional_id AS ProfissionalId,
                    start_at AS InicioEm,
                    end_at AS FimEm,
                    status AS Status,
                    created_at AS CreatedOn,
                    updated_at AS UpdatedOn
                FROM appointments
                WHERE professional_id = @ProfissionalId
                ORDER BY start_at DESC;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Consulta>(sql, new
            {
                ProfissionalId = profissionalId
            });
        }

        public async Task<IEnumerable<Consulta>> ObterAgendaProfissionalAsync(
            Guid profissionalId,
            DateTime data)
        {
            const string sql = """
                SELECT
                    id AS Id,
                    patient_id AS PacienteId,
                    professional_id AS ProfissionalId,
                    start_at AS InicioEm,
                    end_at AS FimEm,
                    status AS Status,
                    created_at AS CreatedOn,
                    updated_at AS UpdatedOn
                FROM appointments
                WHERE professional_id = @ProfissionalId
                  AND DATE(start_at) = DATE(@Data)
                  AND status = 'CONFIRMADO'
                ORDER BY start_at;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Consulta>(sql, new
            {
                ProfissionalId = profissionalId,
                Data = data
            });
        }

        public async Task<bool> ProfissionalPossuiConsultaNoHorarioAsync(
            Guid profissionalId,
            DateTime inicioEm)
        {
            const string sql = """
                SELECT COUNT(1)
                FROM appointments
                WHERE professional_id = @ProfissionalId
                  AND start_at = @InicioEm
                  AND status = 'CONFIRMADO';
            """;

            using var connection = _connectionFactory.CreateConnection();

            var count = await connection.ExecuteScalarAsync<int>(sql, new
            {
                ProfissionalId = profissionalId,
                InicioEm = inicioEm
            });

            return count > 0;
        }

        public async Task<bool> PacientePossuiConsultaComProfissionalNoDiaAsync(
            Guid pacienteId,
            Guid profissionalId,
            DateTime data)
        {
            const string sql = """
                SELECT COUNT(1)
                FROM appointments
                WHERE patient_id = @PacienteId
                  AND professional_id = @ProfissionalId
                  AND DATE(start_at) = DATE(@Data)
                  AND status = 'CONFIRMADO';
            """;

            using var connection = _connectionFactory.CreateConnection();

            var count = await connection.ExecuteScalarAsync<int>(sql, new
            {
                PacienteId = pacienteId,
                ProfissionalId = profissionalId,
                Data = data
            });

            return count > 0;
        }

        public async Task AtualizarAsync(Consulta consulta)
        {
            const string sql = """
                UPDATE appointments
                SET
                    patient_id = @PacienteId,
                    professional_id = @ProfissionalId,
                    start_at = @InicioEm,
                    end_at = @FimEm,
                    status = @Status,
                    updated_at = @UpdatedOn
                WHERE id = @Id;
            """;

            using var connection = _connectionFactory.CreateConnection();
            try
            {

                var linhasAfetadas = await connection.ExecuteAsync(sql, new
                {
                    consulta.Id,
                    consulta.PacienteId,
                    consulta.ProfissionalId,
                    consulta.InicioEm,
                    consulta.FimEm,
                    consulta.Status,
                    UpdatedOn = DateTime.UtcNow
                });
                if (linhasAfetadas == 0)
                    throw new Exception($"Consulta com Id {consulta.Id} não encontrada para atualização.");
            }
            catch (Exception Ex)
            {

                throw;
            }

        }

        public async Task CancelarAsync(Guid id)
        {
            const string sql = """
        UPDATE appointments
        SET
            status = 'CANCELED',
            updated_at = @UpdatedOn
        WHERE id = @Id;
    """;
                using var connection = _connectionFactory.CreateConnection();
            try
            {

    
                var linhasAfetadas = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    UpdatedOn = DateTime.UtcNow
                });
                if (linhasAfetadas == 0)
                    throw new Exception($"Consulta com Id {id} não encontrada para cancelamento.");

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}