using AgendaClinica.Domain.SeedWorks;

namespace AgendaClinica.Domain.Entities
{
    public class Consulta : EntityBase
    {
        public Guid PacienteId { get; private set; }
        public Guid ProfissionalId { get; private set; }
        public DateTime InicioEm { get; private set; }
        public DateTime FimEm { get; private set; }
        public string Status { get; private set; }

        private Consulta() { }

        public Consulta(Guid pacienteId, Guid profissionalId, DateTime inicioEm)
        {
            ValidarHorarioAtendimento(inicioEm);

            PacienteId = pacienteId;
            ProfissionalId = profissionalId;
            InicioEm = inicioEm;
            FimEm = inicioEm.AddMinutes(30);
            Status = "SCHEDULED";
            CreatedOn = DateTime.UtcNow;
        }

        public void Atualizar(Consulta consulta)
        {
            ValidarHorarioAtendimento(consulta.InicioEm);
            PacienteId = consulta.PacienteId;
            ProfissionalId = consulta.ProfissionalId;
            InicioEm = consulta.InicioEm;
            FimEm = InicioEm.AddMinutes(30);
            Status = "SCHEDULED";
            UpdatedOn = DateTime.UtcNow;
        }

        public void Concluir()
        {
            Status = "COMPLETED";
            UpdatedOn = DateTime.UtcNow;
        }

        public void Cancelar()
        {
            Status = "CANCELED";
            UpdatedOn = DateTime.UtcNow;
        }

        private static void ValidarHorarioAtendimento(DateTime inicioEm)
        {
            if (inicioEm.DayOfWeek == DayOfWeek.Saturday || inicioEm.DayOfWeek == DayOfWeek.Sunday)
                throw new InvalidOperationException("Consultas só podem ser agendadas de segunda a sexta-feira.");

            var horario = inicioEm.TimeOfDay;
            var abertura = new TimeSpan(8, 0, 0);
            var ultimoHorarioValido = new TimeSpan(17, 30, 0);

            if (horario < abertura || horario > ultimoHorarioValido)
                throw new InvalidOperationException("Consultas devem ser agendadas entre 08:00 e 18:00.");

            if (inicioEm.Minute != 0 && inicioEm.Minute != 30)
                throw new InvalidOperationException("Consultas devem iniciar em horários de 30 em 30 minutos.");
        }
    }
}
