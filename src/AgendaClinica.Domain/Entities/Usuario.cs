using AgendaClinica.Domain.SeedWorks;

public sealed class Usuario : EntityBase
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public string Role { get; private set; }

    private Usuario()
    {
    }

    public Usuario(
        string nome,
        string email,
        string senhaHash,
        string role)
    {
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Role = role;
        CreatedOn = DateTime.UtcNow;
    }

    public Usuario(
        Guid id,
        string nome,
        string email,
        string senhaHash,
        string role) : base(id)
    {
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Role = role;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Atualizar(
        string nome,
        string email,
        string senhaHash,
        string role)
    {
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Role = role;
        UpdatedOn = DateTime.UtcNow;
    }
    
    public void AtualizarPerfil(string nome)
    {
        Nome = nome;
        UpdatedOn = DateTime.UtcNow;
    }
}