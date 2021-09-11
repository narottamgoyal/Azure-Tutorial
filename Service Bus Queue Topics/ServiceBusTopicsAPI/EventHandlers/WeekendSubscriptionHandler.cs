using AzureEventServiceBus.Events;
using BasicEventBus.Contracts;
using System.Threading.Tasks;

namespace ServiceBusTopicsAPI.Events.EventHandlers
{
    public class WeekendSubscriptionHandler : IEventHandler<TodoTaskCreatedEvent>
    {
        public WeekendSubscriptionHandler()
        {

        }

        public Task HandleAsync(TodoTaskCreatedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
