using System.Threading.Tasks;
using MicroServiceOne;
using MicroserviceTwoConsumer.Services;

namespace MicroserviceTwoConsumer.Events
{
    public class Notification: MicroServiceOne.NotificationEventHandler
    {
        IService _service;

        public Notification(IService service)
        {
            _service = service;
        }

        public override Task Handle(NotificationEvent @event)
        {
            _service.RemoveStock(@event.Message);

            return base.Handle(@event);
        }
    }
}
