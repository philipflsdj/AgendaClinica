using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using global::AgendaClinica.Domain.Entities;


namespace AgendaClinica.Tests.Domain
{

    public class ConsultaTests
    {
        [Fact]
        public void Deve_Criar_Consulta_Com_Dados_Validos()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();
            var profissionalId = Guid.NewGuid();
            var inicioEm = new DateTime(2026, 6, 15, 8, 0, 0);

            // Act
            var consulta = new Consulta(pacienteId, profissionalId, inicioEm);

            // Assert
            consulta.PacienteId.Should().Be(pacienteId);
            consulta.ProfissionalId.Should().Be(profissionalId);
            consulta.InicioEm.Should().Be(inicioEm);
            consulta.FimEm.Should().Be(inicioEm.AddMinutes(30));
            consulta.Status.Should().Be("SCHEDULED");
            consulta.CreatedOn.Should().NotBe(default);
        }

        [Fact]
        public void Deve_Definir_FimEm_Com_30_Minutos_Apos_Inicio()
        {
            // Arrange
            var inicioEm = new DateTime(2026, 6, 15, 10, 30, 0);

            // Act
            var consulta = new Consulta(Guid.NewGuid(), Guid.NewGuid(), inicioEm);

            // Assert
            consulta.FimEm.Should().Be(new DateTime(2026, 6, 15, 11, 0, 0));
        }

        [Theory]
        [InlineData(2026, 6, 13, 8, 0)] // sábado
        [InlineData(2026, 6, 14, 8, 0)] // domingo
        public void Nao_Deve_Criar_Consulta_Aos_Finais_De_Semana(
            int ano,
            int mes,
            int dia,
            int hora,
            int minuto)
        {
            // Arrange
            var inicioEm = new DateTime(ano, mes, dia, hora, minuto, 0);

            // Act
            var act = () => new Consulta(Guid.NewGuid(), Guid.NewGuid(), inicioEm);

            // Assert
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Consultas só podem ser agendadas de segunda a sexta-feira.");
        }

        [Theory]
        [InlineData(2026, 6, 15, 7, 30)]
        [InlineData(2026, 6, 15, 18, 0)]
        public void Nao_Deve_Criar_Consulta_Fora_Do_Horario_De_Atendimento(
            int ano,
            int mes,
            int dia,
            int hora,
            int minuto)
        {
            // Arrange
            var inicioEm = new DateTime(ano, mes, dia, hora, minuto, 0);

            // Act
            var act = () => new Consulta(Guid.NewGuid(), Guid.NewGuid(), inicioEm);

            // Assert
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Consultas devem ser agendadas entre 08:00 e 18:00.");
        }

        [Theory]
        [InlineData(2026, 6, 15, 8, 15)]
        [InlineData(2026, 6, 15, 10, 45)]
        public void Nao_Deve_Criar_Consulta_Com_Minuto_Diferente_De_Zero_Ou_Trinta(
            int ano,
            int mes,
            int dia,
            int hora,
            int minuto)
        {
            // Arrange
            var inicioEm = new DateTime(ano, mes, dia, hora, minuto, 0);

            // Act
            var act = () => new Consulta(Guid.NewGuid(), Guid.NewGuid(), inicioEm);

            // Assert
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Consultas devem iniciar em horários de 30 em 30 minutos.");
        }

        [Fact]
        public void Deve_Concluir_Consulta()
        {
            // Arrange
            var consulta = new Consulta(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateTime(2026, 6, 15, 9, 0, 0));

            // Act
            consulta.Concluir();

            // Assert
            consulta.Status.Should().Be("COMPLETED");
            consulta.UpdatedOn.Should().NotBe(default);
        }

        [Fact]
        public void Deve_Cancelar_Consulta()
        {
            // Arrange
            var consulta = new Consulta(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateTime(2026, 6, 15, 9, 0, 0));

            // Act
            consulta.Cancelar();

            // Assert
            consulta.Status.Should().Be("CANCELED");
            consulta.UpdatedOn.Should().NotBe(default);
        }

        [Fact]
        public void Deve_Atualizar_Consulta()
        {
            // Arrange
            var consulta = new Consulta(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateTime(2026, 6, 15, 9, 0, 0));

            var novaConsulta = new Consulta(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateTime(2026, 6, 16, 14, 30, 0));

            // Act
            consulta.Atualizar(novaConsulta);

            // Assert
            consulta.PacienteId.Should().Be(novaConsulta.PacienteId);
            consulta.ProfissionalId.Should().Be(novaConsulta.ProfissionalId);
            consulta.InicioEm.Should().Be(novaConsulta.InicioEm);
            consulta.FimEm.Should().Be(novaConsulta.InicioEm.AddMinutes(30));
            consulta.Status.Should().Be("SCHEDULED");
            consulta.UpdatedOn.Should().NotBe(default);
        }
    }
}
