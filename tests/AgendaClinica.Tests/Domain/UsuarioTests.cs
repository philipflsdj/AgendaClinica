using AgendaClinica.Domain.Entities;
using FluentAssertions;

namespace AgendaClinica.Tests.Domain
{
    public class UsuarioTests
    {
        [Fact]
        public void Deve_Criar_Usuario_Com_Dados_Validos()
        {
            // Arrange
            var nome = "Admin";
            var email = "admin@email.com";
            var senhaHash = "hash123";
            var role = "ADMIN";

            // Act
            var usuario = new Usuario(nome, email, senhaHash, role);

            // Assert
            usuario.Nome.Should().Be(nome);
            usuario.Email.Should().Be(email);
            usuario.SenhaHash.Should().Be(senhaHash);
            usuario.Role.Should().Be(role);
            usuario.CreatedOn.Should().NotBe(default);
        }

        [Fact]
        public void Deve_Criar_Usuario_Com_Id_Informado()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var usuario = new Usuario(
                id,
                "João",
                "joao@email.com",
                "hash456",
                "USER");

            // Assert
            usuario.Id.Should().Be(id);
            usuario.Nome.Should().Be("João");
            usuario.Email.Should().Be("joao@email.com");
            usuario.SenhaHash.Should().Be("hash456");
            usuario.Role.Should().Be("USER");
            usuario.UpdatedOn.Should().NotBe(default);
        }

        [Fact]
        public void Deve_Atualizar_Usuario()
        {
            // Arrange
            var usuario = new Usuario(
                "João",
                "joao@email.com",
                "hash123",
                "USER");

            // Act
            usuario.Atualizar(
                "João Atualizado",
                "joao.atualizado@email.com",
                "hashAtualizado",
                "ADMIN");

            // Assert
            usuario.Nome.Should().Be("João Atualizado");
            usuario.Email.Should().Be("joao.atualizado@email.com");
            usuario.SenhaHash.Should().Be("hashAtualizado");
            usuario.Role.Should().Be("ADMIN");
            usuario.UpdatedOn.Should().NotBe(default);
        }

        [Fact]
        public void Deve_Atualizar_Perfil_Do_Usuario()
        {
            // Arrange
            var usuario = new Usuario(
                "João",
                "joao@email.com",
                "hash123",
                "USER");

            // Act
            usuario.AtualizarPerfil("João Perfil Atualizado");

            // Assert
            usuario.Nome.Should().Be("João Perfil Atualizado");
            usuario.Email.Should().Be("joao@email.com");
            usuario.SenhaHash.Should().Be("hash123");
            usuario.Role.Should().Be("USER");
            usuario.UpdatedOn.Should().NotBe(default);
        }
    }
}
