using Azure.Messaging.ServiceBus;
using BasicEventBus.Contracts;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureEventServiceBus
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/master/src/BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.cs
    /// </summary>
    public class EventBusService : IEventBusService
    {
        private const string TOPIC_PATH = "mytopic";
        private readonly IEventBusSubscriptionsManager _eventBusSubscriptionsManager;
        private readonly string ConnectionString = "";
        private readonly ServiceBusSender _clientSender;

        public EventBusService(IEventBusSubscriptionsManager eventBusSubscriptionsManager)
        {
            this._eventBusSubscriptionsManager = eventBusSubscriptionsManager;
            var client = new ServiceBusClient(ConnectionString);
            _clientSender = client.CreateSender(TOPIC_PATH);
        }

        public void Subscribe<T, TH>()
            where T : IEvent
            where TH : IEventHandler<T>
        {
            _eventBusSubscriptionsManager.AddSubscription<T, TH>();
        }

        async Task IEventBusService.Publish<T>(T @event)
        {
            try
            {
                string eventMessage = JsonSerializer.Serialize(@event);
                ServiceBusMessage message = new ServiceBusMessage(eventMessage);
                await _clientSender.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task IEventBusService.Publish<E, V>(E @event, string key, V value)
        {
            try
            {
                string eventMessage = JsonSerializer.Serialize(@event);
                ServiceBusMessage message = new ServiceBusMessage(eventMessage);
                message.ApplicationProperties.Add(key, value);
                await _clientSender.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
