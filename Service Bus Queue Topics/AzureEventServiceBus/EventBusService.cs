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
        private readonly IEventBusSubscriptionsManager _eventBusSubscriptionsManager;
        private readonly ServiceBusSender _clientSender;

        public EventBusService(IEventBusSubscriptionsManager eventBusSubscriptionsManager)
        {
            this._eventBusSubscriptionsManager = eventBusSubscriptionsManager;
            var client = new ServiceBusClient(Constants.ConnectionString);
            _clientSender = client.CreateSender(Constants.TopicName);
        }

        public void Subscribe<T, TH>(string subscriptionName)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            _eventBusSubscriptionsManager.AddSubscription<T, TH>(subscriptionName);
        }

        async Task IEventBusService.Publish<T>(T @event)
        {
            try
            {
                string eventMessage = JsonSerializer.Serialize(@event);
                ServiceBusMessage message = new ServiceBusMessage(eventMessage);

                foreach (var key in @event.FilterKeys)
                {
                    var d = @event.GetType().GetProperty(key).GetValue(@event);
                    message.ApplicationProperties.Add(key, d);
                }

                await _clientSender.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
