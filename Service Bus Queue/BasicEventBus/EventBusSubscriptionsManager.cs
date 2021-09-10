using BasicEventBus.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BasicEventBus
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/dotnet3-migration/dev-dotnet3/src/BuildingBlocks/EventBus/EventBus/InMemoryEventBusSubscriptionsManager.cs
    /// </summary>
    public class EventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly ConcurrentDictionary<string, List<Type>> _handlers;
        private ConcurrentBag<Type> _eventTypes;
        public EventBusSubscriptionsManager()
        {
            _handlers = new ConcurrentDictionary<string, List<Type>>();
            _eventTypes = new ConcurrentBag<Type>();
        }

        public void AddSubscription<T, TH>()
            where T : IEvent
            where TH : IEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(TH), eventName);
            _eventTypes.Add(typeof(T));
        }

        private void DoAddSubscription(Type handlerType, string eventName)
        {
            if (_handlers.ContainsKey(eventName) && _handlers[eventName].Any(s => s == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            _handlers.AddOrUpdate(eventName,
                x => new List<Type>() { handlerType },
                (k, v) =>
                {
                    v.Add(handlerType);
                    return v;
                });
        }

        public IEnumerable<Type> GetHandlersForEvent<T>() where T : IEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }
        public IEnumerable<Type> GetHandlersForEvent(string eventName) => _handlers[eventName];

        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }
    }
}
