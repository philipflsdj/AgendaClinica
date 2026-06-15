using AgendaClinica.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Tests.Domain
{
    public class PacienteTests
    {
        [Fact]
        public void Deve_Criar_Paciente_Com_Dados_Validos()
        {
            // Arrange
            var nome = "João da Silva";
            var documento = "12345678900";
            var email = "joao@email.com";
            var telefone = "11999999999";

            // Act
            var paciente = new Paciente(nome, documento, email, telefone);

            // Assert
            paciente.NomeCompleto.Should().Be(nome);
            paciente.Documento.Should().Be(documento);
            paciente.Email.Should().Be(email);
            paciente.Telefone.Should().Be(telefone);
        }

        [Fact]
        public void Deve_Criar_Paciente_A_Partir_De_Outro_Paciente()
        {
            // Arrange
            var pacienteOrigem = new Paciente(
                "Maria Oliveira",
                "98765432100",
                "maria@email.com",
                "11888888888");

            // Act
            var paciente = new Paciente(pacienteOrigem);

            // Assert
            paciente.NomeCompleto.Should().Be(pacienteOrigem.NomeCompleto);
            paciente.Documento.Should().Be(pacienteOrigem.Documento);
            paciente.Email.Should().Be(pacienteOrigem.Email);
            paciente.Telefone.Should().Be(pacienteOrigem.Telefone);
            paciente.CreatedBy.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void Deve_Atualizar_Paciente()
        {
            // Arrange
            var paciente = new Paciente(
                "João da Silva",
                "12345678900",
                "joao@email.com",
                "11999999999");

            var dadosAtualizados = new Paciente(
                "João Pereira",
                "11122233344",
                "joao.pereira@email.com",
                "11777777777");

            // Act
            paciente.Atualizar(dadosAtualizados);

            // Assert
            paciente.NomeCompleto.Should().Be(dadosAtualizados.NomeCompleto);
            paciente.Documento.Should().Be(dadosAtualizados.Documento);
            paciente.Email.Should().Be(dadosAtualizados.Email);
            paciente.Telefone.Should().Be(dadosAtualizados.Telefone);
            paciente.UpdatedBy.Should().NotBeNullOrWhiteSpace();
        }
    }
}
