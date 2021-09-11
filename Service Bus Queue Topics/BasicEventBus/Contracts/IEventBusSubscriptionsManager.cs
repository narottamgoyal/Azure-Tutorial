using System;
using System.Collections.Generic;

namespace BasicEventBus.Contracts
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/dotnet3-migration/dev-dotnet3/src/BuildingBlocks/EventBus/EventBus/IEventBusSubscriptionsManager.cs
    /// </summary>
    public interface IEventBusSubscriptionsManager
    {
        void AddSubscription<T, TH>(string subscriptionName) where T : IEvent
                                       where TH : IEventHandler<T>;
        IEnumerable<Type> GetHandlersForEvent<T>(string subscriptionName) where T : IEvent;
        IEnumerable<Type> GetHandlersForEvent(string eventName, string subscriptionName);
        Type GetEventTypeByName(string eventName);
    }
}
