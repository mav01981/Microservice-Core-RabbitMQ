using MicroServiceOne;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System.Text;

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

        public IActionResult Get()
        {
            return Content("<html><body>Publish API version 1.0</body></html>", "text/html", Encoding.UTF8);
        }

        [HttpPost]
        public void CreateOrder([FromBody] OrderDto model)
        {
            var @order = new NotificationEvent(1, "Test");

            _eventBus.Publish(@order);
        }
    }
}