using AzureEventServiceBus.Events;
using BasicEventBus.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var @event = new TodoTaskCreatedEvent
            {
                Day = day.ToString(),
                Message = message,
            };
            eventBusService.Publish(@event);
            return StatusCode(StatusCodes.Status200OK);
        }
    }

    public enum Days { Sunday, Saturday, Otherday }
}
