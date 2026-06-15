using AgendaClinica.Domain.Entities;
using FluentAssertions;

namespace AgendaClinica.Tests.Domain
{
    public class ProfissionalSaudeTests
    {
        [Fact]
        public void Deve_Criar_Profissional_Com_Dados_Validos()
        {
            // Arrange
            var nome = "Dra. Ana Souza";
            var especialidade = "Cardiologia";
            var numeroRegistro = "CRM123456";

            // Act
            var profissional = new ProfissionalSaude(nome, especialidade, numeroRegistro);

            // Assert
            profissional.Id.Should().NotBe(Guid.Empty);
            profissional.NomeCompleto.Should().Be(nome);
            profissional.Especialidade.Should().Be(especialidade);
            profissional.NumeroRegistro.Should().Be(numeroRegistro);
            profissional.CreatedBy.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void Deve_Criar_Profissional_A_Partir_De_Outro_Profissional()
        {
            // Arrange
            var profissionalOrigem = new ProfissionalSaude(
                "Dr. Carlos Lima",
                "Ortopedia",
                "CRM987654");

            // Act
            var profissional = new ProfissionalSaude(profissionalOrigem);

            // Assert
            profissional.NomeCompleto.Should().Be(profissionalOrigem.NomeCompleto);
            profissional.Especialidade.Should().Be(profissionalOrigem.Especialidade);
            profissional.NumeroRegistro.Should().Be(profissionalOrigem.NumeroRegistro);
            profissional.CreatedBy.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void Deve_Atualizar_Profissional()
        {
            // Arrange
            var profissional = new ProfissionalSaude(
                "Dra. Ana Souza",
                "Cardiologia",
                "CRM123456");

            var dadosAtualizados = new ProfissionalSaude(
                "Dra. Ana Souza Lima",
                "Clínica Geral",
                "CRM654321");

            // Act
            profissional.Atualizar(dadosAtualizados);

            // Assert
            profissional.NomeCompleto.Should().Be(dadosAtualizados.NomeCompleto);
            profissional.Especialidade.Should().Be(dadosAtualizados.Especialidade);
            profissional.NumeroRegistro.Should().Be(dadosAtualizados.NumeroRegistro);
            profissional.UpdatedBy.Should().NotBeNullOrWhiteSpace();
        }
    }
}
