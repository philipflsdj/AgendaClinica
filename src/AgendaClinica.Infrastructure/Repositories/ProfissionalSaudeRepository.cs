using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Infrastructure.Data;
using Dapper;

namespace AgendaClinica.Infrastructure.Repositories
{
    public class ProfissionalSaudeRepository : IProfissionalSaudeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ProfissionalSaudeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AdicionarAsync(ProfissionalSaude profissional)
        {
            const string sql = """
                INSERT INTO health_professionals
                (
                    id,
                    full_name,
                    specialty,
                    registration_number,
                    created_at
                )
                VALUES
                (
                    @Id,
                    @NomeCompleto,
                    @Especialidade,
                    @NumeroRegistro,
                    @CreatedOn
                );
            """;
            profissional.CreatedOn ??= DateTime.UtcNow;

            using var connection = _connectionFactory.CreateConnection();
            try
            {
            await connection.ExecuteAsync(sql, profissional);

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<ProfissionalSaude?> ObterPorIdAsync(Guid id)
        {
            const string sql = """
                SELECT
                    id AS Id,
                    full_name AS NomeCompleto,
                    specialty AS Especialidade,
                    registration_number AS NumeroRegistro,
                    created_at AS CriadoEm
                FROM health_professionals
                WHERE id = @Id;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<ProfissionalSaude>(sql, new
            {
                Id = id
            });
        }

        public async Task<ProfissionalSaude?> ObterPorNumeroRegistroAsync(string numeroRegistro)
        {
            const string sql = """
                SELECT
                    id AS Id,
                    full_name AS NomeCompleto,
                    specialty AS Especialidade,
                    registration_number AS NumeroRegistro,
                    created_at AS CriadoEm
                FROM health_professionals
                WHERE registration_number = @NumeroRegistro;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<ProfissionalSaude>(sql, new
            {
                NumeroRegistro = numeroRegistro
            });
        }

        public async Task<IEnumerable<ProfissionalSaude>> ObterTodosAsync()
        {
            const string sql = """
                SELECT
                    id AS Id,
                    full_name AS NomeCompleto,
                    specialty AS Especialidade,
                    registration_number AS NumeroRegistro,
                    created_at AS CriadoEm
                FROM health_professionals
                ORDER BY full_name;
            """;

            using var connection = _connectionFactory.CreateConnection();
            var teste = connection.QueryAsync<ProfissionalSaude>(sql);
            return await teste;
        }

        public async Task<bool> NumeroRegistroJaExisteAsync(string numeroRegistro)
        {
            const string sql = """
                SELECT COUNT(1)
                FROM health_professionals
                WHERE registration_number = @NumeroRegistro;
            """;

            using var connection = _connectionFactory.CreateConnection();

            var count = await connection.ExecuteScalarAsync<int>(sql, new
            {
                NumeroRegistro = numeroRegistro
            });

            return count > 0;
        }

        public async Task AtualizarAsync(ProfissionalSaude profissional)
        {
            const string sql = """
                UPDATE health_professionals
                SET
                    full_name = @NomeCompleto,
                    specialty = @Especialidade,
                    registration_number = @NumeroRegistro
                WHERE id = @Id;
            """;

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(sql, profissional);
        }

        public async Task RemoverAsync(Guid id)
        {
            const string sql = """
                DELETE FROM health_professionals
                WHERE id = @Id;
            """;

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(sql, new
            {
                Id = id
            });
        }
    }
}