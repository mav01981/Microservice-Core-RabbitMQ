using MicroServiceOne;
using Microsoft.AspNetCore.Mvc;

namespace Publisher.Controllers
{
    [Produces("application/json")]
    [Route("api/Publish")]
    public class PublishController : Controller
    {
        private readonly IEventBus _eventBus;

        public PublishController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        // GET api/Publish
        [HttpGet]
        public void Get()
        {
            var @event = new NotificationEvent(1, "This is a new Notification Event !");

            _eventBus.Publish(@event);
        }
    }
}