using BasicEventBus.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBusTopicsAPI.Events;

namespace ServiceBusTopicsAPI.Controllers
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

        [HttpPost("{message}/{day}")]
        public IActionResult Post(string message, Days day)
        {
            eventBusService.Publish<SampleTodoItemEvent, string>(
                new SampleTodoItemEvent
                {
                    Day = day.ToString(),
                    Message = message,
                }, "Day", day.ToString());
            return StatusCode(StatusCodes.Status200OK);
        }
    }

    public enum Days { Sunday, Saturday, Otherday }
}
