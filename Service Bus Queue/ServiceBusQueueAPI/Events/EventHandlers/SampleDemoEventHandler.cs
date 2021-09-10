using BasicEventBus.Contracts;
using System.Threading.Tasks;

namespace ServiceBusQueueAPI.Events.EventHandlers
{
    public class SampleDemoEventHandler : IEventHandler<SampleDemoEvent>
    {
        public SampleDemoEventHandler()
        {

        }

        public async Task HandleAsync(SampleDemoEvent @event)
        {

        }
    }
}
