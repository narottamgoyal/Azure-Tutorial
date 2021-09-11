using BasicEventBus.Contracts;

namespace ServiceBusTopicsAPI.Events
{
    public class SampleTodoItemEvent : IEvent
    {
        public SampleTodoItemEvent()
        {
            EventName = GetType().Name;
        }

        public string EventName { get; set; }
        public string Message { get; set; }
        public string Day { get; set; }
    }
}
