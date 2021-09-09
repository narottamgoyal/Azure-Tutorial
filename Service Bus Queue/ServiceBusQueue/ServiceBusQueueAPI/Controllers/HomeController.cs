using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceBusQueueAPI.Events;
using ServiceBusQueueAPI.Events.Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            eventPublisherService.PublishMessageAsync<MyFirstQueue>(
                new MyFirstQueue
                {
                    Name = username
                });
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
