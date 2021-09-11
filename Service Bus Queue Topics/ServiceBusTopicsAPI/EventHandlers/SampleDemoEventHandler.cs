using BasicEventBus.Contracts;
using System.Threading.Tasks;

namespace ServiceBusTopicsAPI.Events.EventHandlers
{
    public class SampleDemoEventHandler : IEventHandler<SampleTodoItemEvent>
    {
        public SampleDemoEventHandler()
        {

        }

        public Task HandleAsync(SampleTodoItemEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
