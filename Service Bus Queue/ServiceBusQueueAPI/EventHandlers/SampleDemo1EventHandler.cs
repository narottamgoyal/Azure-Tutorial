using BasicEventBus.Contracts;
using System.Threading.Tasks;

namespace ServiceBusQueueAPI.Events.EventHandlers
{
    public class SampleDemo1EventHandler : IEventHandler<SampleDemo1Event>
    {
        public SampleDemo1EventHandler()
        {

        }

        public Task HandleAsync(SampleDemo1Event @event)
        {
            return Task.CompletedTask;
        }
    }
}
