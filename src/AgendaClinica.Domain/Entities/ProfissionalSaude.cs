using AgendaClinica.Domain.SeedWorks;


namespace AgendaClinica.Domain.Entities
{
    public sealed class ProfissionalSaude : EntityBase
    {
        public string NomeCompleto { get; private set; }
        public string Especialidade { get; private set; }
        public string NumeroRegistro { get; private set; }

        public ProfissionalSaude()
        {
        }

        public ProfissionalSaude(ProfissionalSaude profissionalSaude)
        {
            NomeCompleto = profissionalSaude.NomeCompleto;
            Especialidade = profissionalSaude.Especialidade;
            NumeroRegistro = profissionalSaude.NumeroRegistro;
            CreatedBy = DateTime.UtcNow.ToString();
        }

        public void Atualizar(ProfissionalSaude profissionalSaude)
        {
            NomeCompleto = profissionalSaude.NomeCompleto;
            Especialidade = profissionalSaude.Especialidade;
            NumeroRegistro = profissionalSaude.NumeroRegistro;
            UpdatedBy = DateTime.UtcNow.ToString();
        }

        public ProfissionalSaude(
    string nomeCompleto,
    string especialidade,
    string numeroRegistro)
        {
            Id = Guid.NewGuid();
            NomeCompleto = nomeCompleto;
            Especialidade = especialidade;
            NumeroRegistro = numeroRegistro;
            CreatedBy = DateTime.UtcNow.ToString();
        }
    }
}
