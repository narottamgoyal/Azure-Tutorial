using BasicEventBus.Contracts;
using System.Collections.Generic;

namespace AzureEventServiceBus.Events
{
    public class TodoTaskCreatedEvent : IEvent
    {
        public TodoTaskCreatedEvent()
        {
            EventName = GetType().Name;
            FilterKeys = new List<string>
            {
                nameof(Day)
            };
        }

        public IList<string> FilterKeys { get; set; }
        public string EventName { get; set; }
        public string Message { get; set; }
        public string Day { get; set; }
    }
}
