using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Infrastructure.Data;
using Dapper;

namespace AgendaClinica.Infrastructure.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PacienteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AdicionarAsync(Paciente paciente)
        {
            const string sql = """
                INSERT INTO patients
                (
                    id,
                    full_name,
                    document,
                    email,
                    phone,
                    created_at
                )
                VALUES
                (
                    @Id,
                    @NomeCompleto,
                    @Documento,
                    @Email,
                    @Telefone,
                    @CreatedOn
                );
            """;
            paciente.CreatedOn ??= DateTime.UtcNow;
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, paciente);

        }

        public async Task<Paciente?> ObterPorIdAsync(Guid id)
        {
            const string sql = """
                SELECT
                    id AS Id,
                    full_name AS NomeCompleto,
                    document AS Documento,
                    email AS Email,
                    phone AS Telefone,
                    created_at AS CriadoEm
                FROM patients
                WHERE id = @Id;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Paciente>(sql, new
            {
                Id = id
            });
        }

        public async Task<Paciente?> ObterPorDocumentoAsync(string documento)
        {
            const string sql = """
                SELECT
                    id AS Id,
                    full_name AS NomeCompleto,
                    document AS Documento,
                    email AS Email,
                    phone AS Telefone,
                    created_at AS CriadoEm
                FROM patients
                WHERE document = @Documento;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Paciente>(sql, new
            {
                Documento = documento
            });
        }

        public async Task<IEnumerable<Paciente>> ObterTodosAsync()
        {
            const string sql = """
                SELECT
                    id AS Id,
                    full_name AS NomeCompleto,
                    document AS Documento,
                    email AS Email,
                    phone AS Telefone,
                    created_at AS CriadoEm
                FROM patients
                ORDER BY full_name;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Paciente>(sql);
        }

        public async Task<bool> DocumentoJaExisteAsync(string documento)
        {
            const string sql = """
                SELECT COUNT(1)
                FROM patients
                WHERE document = @Documento;
            """;

            using var connection = _connectionFactory.CreateConnection();

            var count = await connection.ExecuteScalarAsync<int>(sql, new
            {
                Documento = documento
            });

            return count > 0;
        }

        public async Task<bool> EmailJaExisteAsync(string email)
        {
            const string sql = """
                SELECT COUNT(1)
                FROM patients
                WHERE email = @Email;
            """;

            using var connection = _connectionFactory.CreateConnection();

            var count = await connection.ExecuteScalarAsync<int>(sql, new
            {
                Email = email
            });

            return count > 0;
        }

        public async Task AtualizarAsync(Paciente paciente)
        {
            const string sql = """
            UPDATE patients
                SET
                    full_name = @NomeCompleto,
                    document = @Documento,
                    email = @Email,
                    phone = @Telefone
                WHERE id = @Id;
            """;

            using var connection = _connectionFactory.CreateConnection();

            var linhasAfetadas = await connection.ExecuteAsync(sql, paciente);

            if (linhasAfetadas == 0)
                throw new Exception($"Paciente com Id {paciente.Id} não encontrado para atualização.");
        }

        public async Task RemoverAsync(Guid id)
        {
            const string sql = """
                DELETE FROM patients
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