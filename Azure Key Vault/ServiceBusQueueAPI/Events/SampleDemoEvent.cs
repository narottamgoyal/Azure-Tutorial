namespace ServiceBusQueueAPI.Events
{
    public interface IEvent
    {
        string EventName { get; set; }
    }

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
