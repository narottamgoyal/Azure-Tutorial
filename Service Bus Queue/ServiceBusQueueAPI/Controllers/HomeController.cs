using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBusQueueAPI.Events;
using ServiceBusQueueAPI.Events.Publisher;

namespace ServiceBusQueueAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IEventPublisherService eventPublisherService;

        public HomeController(IEventPublisherService eventPublisherService)
        {
            this.eventPublisherService = eventPublisherService;
        }

        [HttpPost("username")]
        public IActionResult Post(string username)
        {
            eventPublisherService.PublishMessageAsync<SampleDemoEvent>(
                new SampleDemoEvent
                {
                    Message = username
                });
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
