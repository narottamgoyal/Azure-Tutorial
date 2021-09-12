using BasicEventBus.Contracts;

namespace ServiceBusQueueAPI.Events
{
    public class SampleDemo1Event : IEvent
    {
        public SampleDemo1Event()
        {
            EventName = GetType().Name;
        }

        public string EventName { get; set; }
        public string Message { get; set; }
    }
}
