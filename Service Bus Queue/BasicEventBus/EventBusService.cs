using BasicEventBus.Contracts;

namespace BasicEventBus
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/master/src/BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.cs
    /// </summary>
    public class EventBusService : IEventBusService
    {
        private readonly IEventBusSubscriptionsManager _eventBusSubscriptionsManager;

        public EventBusService(IEventBusSubscriptionsManager eventBusSubscriptionsManager)
        {
            this._eventBusSubscriptionsManager = eventBusSubscriptionsManager;
        }

        public void Subscribe<T, TH>()
            where T : IEvent
            where TH : IEventHandler<T>
        {
            _eventBusSubscriptionsManager.AddSubscription<T, TH>();
        }
    }
}
