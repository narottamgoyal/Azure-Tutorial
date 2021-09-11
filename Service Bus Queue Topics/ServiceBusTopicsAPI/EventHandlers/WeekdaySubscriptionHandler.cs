using AzureEventServiceBus.Events;
using BasicEventBus.Contracts;
using System.Threading.Tasks;

namespace ServiceBusTopicsAPI.Events.EventHandlers
{
    public class WeekdaySubscriptionHandler : IEventHandler<TodoTaskCreatedEvent>
    {
        public WeekdaySubscriptionHandler()
        {

        }

        public Task HandleAsync(TodoTaskCreatedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
