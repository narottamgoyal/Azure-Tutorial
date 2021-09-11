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

        public void AddSubscription<T, TH>(string subscriptionName)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(TH), GetHandlerKey(eventName, subscriptionName));
            _eventTypes.Add(typeof(T));
        }

        private static string GetHandlerKey(string eventName, string subscriptionName)
        {
            return $"{eventName}_{subscriptionName}";
        }

        private void DoAddSubscription(Type handlerType, string handlerKey)
        {
            if (_handlers.ContainsKey(handlerKey) && _handlers[handlerKey].Any(s => s == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{handlerKey}'", nameof(handlerType));
            }

            _handlers.AddOrUpdate(handlerKey,
                x => new List<Type>() { handlerType },
                (k, v) =>
                {
                    v.Add(handlerType);
                    return v;
                });
        }

        public IEnumerable<Type> GetHandlersForEvent<T>(string subscriptionName) where T : IEvent
        {
            var eventName = GetEventKey<T>();
            return GetHandlersForEvent(eventName, subscriptionName);
        }

        public IEnumerable<Type> GetHandlersForEvent(string eventName, string subscriptionName)
            => _handlers[GetHandlerKey(eventName, subscriptionName)];

        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }
    }
}
