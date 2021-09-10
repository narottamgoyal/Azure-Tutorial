using System;
using System.Collections.Generic;

namespace BasicEventBus.Contracts
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/dotnet3-migration/dev-dotnet3/src/BuildingBlocks/EventBus/EventBus/IEventBusSubscriptionsManager.cs
    /// </summary>
    public interface IEventBusSubscriptionsManager
    {
        void AddSubscription<T, TH>() where T : IEvent
                                       where TH : IEventHandler<T>;
        IEnumerable<Type> GetHandlersForEvent<T>() where T : IEvent;
        IEnumerable<Type> GetHandlersForEvent(string eventName);
        Type GetEventTypeByName(string eventName);

        //bool IsEmpty { get; }
        //event EventHandler<string> OnEventRemoved;
        //void RemoveSubscription<T, TH>() where TH : IBaseEventHandler<T>
        //                                    where T : IBaseEvent;
        //bool HasSubscriptionsForEvent<T>() where T : IBaseEvent;
        //bool HasSubscriptionsForEvent(string eventName);
        //void Clear();

        //string GetEventKey<T>();
    }
}
