using Microsoft.AspNetCore.Mvc;

namespace MicroServiceOne.Controllers
{
    [Produces("application/json")]
    [Route("api/Subscriber")]
    public class SubscriberController : Controller
    {
        private readonly IEventBus _eventBus;

        public SubscriberController(IEventBus eventbus)
        {
         

        }
    }
}