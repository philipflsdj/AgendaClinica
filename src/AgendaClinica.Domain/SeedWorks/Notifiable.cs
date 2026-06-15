using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaClinica.Domain.SeedWorks
{
    public abstract class Notifiable
    {
        private readonly List<string> _notifications = new();

        public IReadOnlyCollection<string> Notifications => _notifications;

        public bool IsValid => !_notifications.Any();

        protected void AddNotification(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
                _notifications.Add(message);
        }

        protected void AddNotifications(IEnumerable<string> messages)
        {
            foreach (var message in messages)
                AddNotification(message);
        }

        public void ClearNotifications() => _notifications.Clear();
    }
}
