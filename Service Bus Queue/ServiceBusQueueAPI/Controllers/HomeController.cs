using BasicEventBus.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBusQueueAPI.Events;

namespace ServiceBusQueueAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IEventBusService eventBusService;

        public HomeController(IEventBusService eventPublisherService)
        {
            this.eventBusService = eventPublisherService;
        }

        [HttpPost("message")]
        public IActionResult Post(string message)
        {
            eventBusService.Publish<SampleDemo1Event>(
                new SampleDemo1Event
                {
                    Message = message
                });
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
