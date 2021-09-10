using BasicEventBus.Contracts;

namespace ServiceBusQueueAPI.Events
{
    public class SampleDemoEvent : IEvent
    {
        public SampleDemoEvent()
        {
            EventName = GetType().Name;
        }

        public string EventName { get; set; }
        public string Message { get; set; }
    }
}
