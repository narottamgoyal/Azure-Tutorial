using BasicEventBus.Contracts;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureEventServiceBus
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/master/src/BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.cs
    /// </summary>
    public class EventBusService : IEventBusService
    {
        private readonly IEventBusSubscriptionsManager _eventBusSubscriptionsManager;
        private readonly string ConnectionString = "";

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

        async Task IEventBusService.Publish<T>(T eventMessage)
        {
            var queueClient = new QueueClient(ConnectionString, typeof(T).FullName);
            string messageBody = JsonSerializer.Serialize(eventMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            await queueClient.SendAsync(message);
        }
    }
}
