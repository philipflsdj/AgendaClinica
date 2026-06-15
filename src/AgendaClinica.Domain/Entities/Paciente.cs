using AgendaClinica.Domain.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Domain.Entities
{
    public sealed class Paciente : EntityBase
    {
        public string NomeCompleto { get; private set; }
        public string Documento { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }

        public Paciente()
        {
        }
        public Paciente(Paciente paciente)
        {
            NomeCompleto = paciente.NomeCompleto;
            Documento = paciente.Documento;
            Email = paciente.Email;
            Telefone = paciente.Telefone;
            CreatedBy = DateTime.UtcNow.ToString();
        }

        public void Atualizar(Paciente paciente)
        {
            NomeCompleto = paciente.NomeCompleto;
            Documento = paciente.Documento;
            Email = paciente.Email;
            Telefone = paciente.Telefone;
            UpdatedBy = DateTime.UtcNow.ToString();
        }

        public Paciente(string nomeCompleto, string documento, string email, string telefone)
        {
            NomeCompleto = nomeCompleto;
            Documento = documento;
            Email = email;
            Telefone = telefone;
        }
    }
}
