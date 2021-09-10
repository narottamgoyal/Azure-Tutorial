using BasicEventBus.Contracts;
using System.Threading.Tasks;

namespace ServiceBusQueueAPI.Events.EventHandlers
{
    public class SampleDemoEventHandler : IEventHandler<SampleDemoEvent>
    {
        public SampleDemoEventHandler()
        {

        }

        public Task HandleAsync(SampleDemoEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
