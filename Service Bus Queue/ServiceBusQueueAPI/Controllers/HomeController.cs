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

        [HttpPost("username")]
        public IActionResult Post(string username)
        {
            eventBusService.Publish<SampleDemoEvent>(
                new SampleDemoEvent
                {
                    Message = username
                });
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
