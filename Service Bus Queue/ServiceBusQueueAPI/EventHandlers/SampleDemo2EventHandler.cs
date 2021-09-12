using BasicEventBus.Contracts;
using System.Threading.Tasks;

namespace ServiceBusQueueAPI.Events.EventHandlers
{
    public class SampleDemo2EventHandler : IEventHandler<SampleDemo2Event>
    {
        public SampleDemo2EventHandler()
        {

        }

        public Task HandleAsync(SampleDemo2Event @event)
        {
            return Task.CompletedTask;
        }
    }
}
