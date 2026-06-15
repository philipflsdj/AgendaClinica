using AgendaClinica.Application.Configurations.Notificacoes;

namespace AgendaClinica.Application.Configurations.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        Task Handle(Notificacao notificacao, CancellationToken cancellationToken);
    }
}
