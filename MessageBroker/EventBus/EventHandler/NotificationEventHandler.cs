using System.Diagnostics;
using System.Threading.Tasks;

namespace MicroServiceOne
{
    public class NotificationEventHandler :
        IIntegrationEventHandler<NotificationEvent>
    {
        public Task Handle(NotificationEvent @event)
        {
            //To do Update other Micros-Service.
            Debug.WriteLine($"{@event.Message}");

            return Task.FromResult(true);
        }
    }
}
