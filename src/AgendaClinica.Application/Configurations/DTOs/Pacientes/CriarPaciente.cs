using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Application.Configurations.DTOs.Pacientes
{
    public class CriarPaciente
    {
        public string? nomeCompleto { get; set; }
        public string? documento { get; set; }
        public string? email { get; set; }
        public string? telefone { get; set; }
        public string? criadoEm { get; set; }
        
    }
}
