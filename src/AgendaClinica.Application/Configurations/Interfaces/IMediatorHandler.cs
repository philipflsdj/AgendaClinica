using AgendaClinica.Application.Configurations.Notificacoes;
using MediatR;


namespace AgendaClinica.Application.Configurations.Interfaces
{
    public interface IMediatorHandler
    {
        Task PublicarNotificacao<T>(T notificacao) where T : Notificacao;
     }
}
