using AgendaClinica.Application.Configurations.Interfaces;
using MediatR;


namespace AgendaClinica.Application.Configurations.Notificacoes
{
    public class Notificador : INotificationHandler<Notificacao>, INotificador
    {
        private List<Notificacao> _notificacoes;

        public Notificador()
        {
            _notificacoes = new List<Notificacao>();
        }

        public Task Handle(Notificacao notificacao, CancellationToken cancellationToken)
        {
            _notificacoes.Add(notificacao);
            return Task.CompletedTask;
        }

        public List<Notificacao> ObterNotificacoes()
        {
            return _notificacoes;
        }

        public bool TemNotificacao()
        {
            return _notificacoes.Any();
        }
    }
}
