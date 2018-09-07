using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MicroServiceOne.Controllers
{
    [Produces("application/json")]
    [Route("api/Subscriber")]
    public class StockController : Controller
    {
        private readonly IEventBus _eventBus;

        public StockController(IEventBus eventbus)
        {
         

        }

        public IActionResult Get()
        {
            return Content("<html><body>Stock API version 1.0</body></html>", "text/html", Encoding.UTF8);
        }
    }
}