using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MicroServiceOne
{
    public class NotificationEventHandler :
        IIntegrationEventHandler<NotificationEvent>
    {
        public static event EventHandler NotificationEvent;

        public virtual Task Handle(NotificationEvent @event)
        {
            Debug.WriteLine($"{@event.Message}");

            return Task.FromResult(true);
        }
    }
}
