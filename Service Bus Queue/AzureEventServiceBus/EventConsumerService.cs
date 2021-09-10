using Azure.Messaging.ServiceBus;
using BasicEventBus.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureEventServiceBus
{
    public class EventConsumerService : IEventConsumerService
    {
        private readonly ServiceBusClient _client;
        private List<ServiceBusProcessor> _processors = new List<ServiceBusProcessor>();
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventBusSubscriptionsManager _eventBusSubscriptionsManager;

        public EventConsumerService(IServiceProvider serviceProvider, IEventBusSubscriptionsManager eventBusSubscriptionsManager)
        {
            // this can be set from appsettings
            var connectionString = "";
            _client = new ServiceBusClient(connectionString);
            this._serviceProvider = serviceProvider;
            _eventBusSubscriptionsManager = eventBusSubscriptionsManager;
        }

        public async Task StopListenerAsync()
        {
            _processors.ForEach(async p =>
            {
                await p.CloseAsync();
            });
            await _client.DisposeAsync();
        }

        public async Task RegisterBaseEventHandlerAsync(IList<string> names)
        {
            if (names == null || !names.Any()) return;

            ServiceBusProcessorOptions serviceBusProcessorOptions = new ServiceBusProcessorOptions
            { AutoCompleteMessages = false };

            foreach (var name in names)
            {
                var processors = _client.CreateProcessor(name, serviceBusProcessorOptions);
                processors.ProcessMessageAsync += ProcessMessagesAsync;
                processors.ProcessErrorAsync += ProcessErrorAsync;
                await processors.StartProcessingAsync();
                _processors.Add(processors);
            }
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs arg)
        {
            string jsonString = Encoding.UTF8.GetString(arg.Message.Body);
            dynamic data = JObject.Parse(jsonString);
            var eventName = data.EventName?.ToString();

            var messageEventType = _eventBusSubscriptionsManager.GetEventTypeByName(eventName);
            dynamic deserialized = JsonConvert.DeserializeObject(jsonString, messageEventType);


            var handlerTypes = _eventBusSubscriptionsManager.GetHandlersForEvent(eventName);
            foreach (var handlerType in handlerTypes)
            {
                dynamic handlerInstance = _serviceProvider.GetService(handlerType);
                // http://techxposer.com/2018/01/03/missing-compiler-required-member-microsoft-csharp-runtimebinder-csharpargumentinfo-create-solved/
                // https://social.msdn.microsoft.com/Forums/sharepoint/en-US/2b855369-a721-4010-9e33-72d699960994/how-to-fix-missing-compiler-member-error-microsoftcsharpruntimebindercsharpargumentinfocreate?forum=visualstudiogeneral
                await handlerInstance?.HandleAsync(deserialized);
            }
            await arg.CompleteMessageAsync(arg.Message);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            // log or do something
            return Task.CompletedTask;
        }
    }
}
