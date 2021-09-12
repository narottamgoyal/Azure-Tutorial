using BasicEventBus.Contracts;

namespace ServiceBusQueueAPI.Events
{
    public class SampleDemo2Event : IEvent
    {
        public SampleDemo2Event()
        {
            EventName = GetType().Name;
        }

        public string EventName { get; set; }
        public string Message { get; set; }
    }
}
