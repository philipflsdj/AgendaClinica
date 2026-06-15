using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Application.Configurations.DTOs.Consultas
{
    public class CriarConsultaRequest
    {
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public DateTime InicioEm { get; set; }
    }
}
