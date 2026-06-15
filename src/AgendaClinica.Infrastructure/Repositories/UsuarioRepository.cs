using AgendaClinica.Domain.Contracts.Repositories;
using AgendaClinica.Domain.Entities;
using AgendaClinica.Infrastructure.Data;
using Dapper;

namespace AgendaClinica.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UsuarioRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            const string sql = """
                INSERT INTO users
                (
                    id,
                    name,
                    email,
                    password_hash,
                    role,
                    created_at
                )
                VALUES
                (
                    @Id,
                    @Nome,
                    @Email,
                    @SenhaHash,
                    @Role,
                    @CreatedOn
                );
            """;

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(sql, new
            {
                usuario.Id,
                Nome = usuario.Nome,
                usuario.Email,
                SenhaHash = usuario.SenhaHash,
                usuario.Role,
                CreatedOn = usuario.CreatedOn ?? DateTime.UtcNow
            });
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            const string sql = """
                UPDATE users
                SET
                    name = @Nome,
                    email = @Email,
                    password_hash = @SenhaHash,
                    role = @Role,
                    updated_at = @UpdatedOn
                WHERE id = @Id;
            """;

            using var connection = _connectionFactory.CreateConnection();

            var linhasAfetadas = await connection.ExecuteAsync(sql, new
            {
                usuario.Id,
                Nome = usuario.Nome,
                usuario.Email,
                SenhaHash = usuario.SenhaHash,
                usuario.Role,
                UpdatedOn = DateTime.UtcNow
            });

            if (linhasAfetadas == 0)
                throw new Exception($"Usuário com Id {usuario.Id} não encontrado para atualização.");
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            const string sql = """
                SELECT
                    id AS Id,
                    name AS Nome,
                    email AS Email,
                    password_hash AS SenhaHash,
                    role AS Role,
                    created_at AS CreatedOn,
                    updated_at AS UpdatedOn
                FROM users
                WHERE email = @Email;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new
            {
                Email = email
            });
        }

        public async Task<Usuario?> ObterPorIdAsync(Guid id)
        {
            const string sql = """
                SELECT
                    id AS Id,
                    name AS Nome,
                    email AS Email,
                    password_hash AS SenhaHash,
                    role AS Role,
                    created_at AS CreatedOn,
                    updated_at AS UpdatedOn
                FROM users
                WHERE id = @Id;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new
            {
                Id = id
            });
        }

        public async Task<IEnumerable<Usuario>> ObterTodosAsync()
        {
            const string sql = """
                SELECT
                    id AS Id,
                    name AS Nome,
                    email AS Email,
                    password_hash AS SenhaHash,
                    role AS Role,
                    created_at AS CreatedOn,
                    updated_at AS UpdatedOn
                FROM users
                ORDER BY name;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Usuario>(sql);
        }

        public async Task RemoverAsync(Guid id)
        {
            const string sql = """
                DELETE FROM users
                WHERE id = @Id;
            """;

            using var connection = _connectionFactory.CreateConnection();

            var linhasAfetadas = await connection.ExecuteAsync(sql, new
            {
                Id = id
            });

            if (linhasAfetadas == 0)
                throw new Exception($"Usuário com Id {id} não encontrado para remoção.");
        }

        public async Task AtualizarPerfilAsync(Usuario usuario)
        {
            const string sql = """
        UPDATE users
        SET
            name = @Nome,
            updated_at = @UpdatedOn
        WHERE id = @Id;
    """;

            using var connection = _connectionFactory.CreateConnection();

            var linhasAfetadas = await connection.ExecuteAsync(sql, new
            {
                usuario.Id,
                usuario.Nome,
                UpdatedOn = DateTime.UtcNow
            });

            if (linhasAfetadas == 0)
                throw new Exception($"Usuário com Id {usuario.Id} não encontrado para atualização de perfil.");
        }
    }
}