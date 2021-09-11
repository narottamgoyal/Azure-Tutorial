using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using AzureEventServiceBus.Events;
using BasicEventBus.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureEventServiceBus
{
    public class EventConsumerService : IEventConsumerService
    {
        private readonly ServiceBusClient _client;
        private List<ServiceBusProcessor> _processors = new List<ServiceBusProcessor>();
        private readonly IServiceProvider _serviceProvider;
        private readonly ServiceBusAdministrationClient _adminClient;
        private readonly IEventBusSubscriptionsManager _eventBusSubscriptionsManager;

        public EventConsumerService(IServiceProvider serviceProvider, IEventBusSubscriptionsManager eventBusSubscriptionsManager)
        {
            _client = new ServiceBusClient(Constants.ConnectionString);
            this._serviceProvider = serviceProvider;
            _eventBusSubscriptionsManager = eventBusSubscriptionsManager;
            _adminClient = new ServiceBusAdministrationClient(Constants.ConnectionString);
        }

        public async Task UnSubscribeAsync()
        {
            _processors.ForEach(async p =>
            {
                await p.CloseAsync();
            });
            await _client.DisposeAsync();
        }

        public async Task RegisterWeekendSubscriptionAsync()
        {
            ServiceBusProcessorOptions serviceBusProcessorOptions = new ServiceBusProcessorOptions
            { AutoCompleteMessages = false };

            var processors = _client.CreateProcessor(Constants.TopicName, SubscriptionNames.weekend.ToString(), serviceBusProcessorOptions);
            processors.ProcessMessageAsync += WeekendSubscriptionMessagesAsync;
            processors.ProcessErrorAsync += ProcessErrorAsync;
            await processors.StartProcessingAsync();
            _processors.Add(processors);

            var ruleProperties = await GetRuleProperties(SubscriptionNames.weekend.ToString());
            await RemoveDefaultFilters(SubscriptionNames.weekend.ToString(), ruleProperties);
            await AddFilterToWeekend(SubscriptionNames.weekend.ToString(), ruleProperties);
        }

        public async Task RegisterWeekdaySubscriptionAsync()
        {
            ServiceBusProcessorOptions serviceBusProcessorOptions = new ServiceBusProcessorOptions
            { AutoCompleteMessages = false };

            var processors = _client.CreateProcessor(Constants.TopicName, SubscriptionNames.weekday.ToString(), serviceBusProcessorOptions);
            processors.ProcessMessageAsync += WeekdaySubscriptionMessagesAsync;
            processors.ProcessErrorAsync += ProcessErrorAsync;
            await processors.StartProcessingAsync();
            _processors.Add(processors);
            var ruleProperties = await GetRuleProperties(SubscriptionNames.weekday.ToString());
            await RemoveDefaultFilters(SubscriptionNames.weekday.ToString(), ruleProperties);
            await AddFilterToWeekday(SubscriptionNames.weekday.ToString(), ruleProperties);
        }

        private async Task<List<RuleProperties>> GetRuleProperties(string subscriptionName)
        {
            try
            {
                var rules = _adminClient.GetRulesAsync(Constants.TopicName, subscriptionName);
                var ruleProperties = new List<RuleProperties>();
                await foreach (var rule in rules) { ruleProperties.Add(rule); }
                return ruleProperties;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private async Task AddFilterToWeekend(string subscriptionName, List<RuleProperties> ruleProperties)
        {
            //var customKeyValueFilter = new CorrelationRuleFilter();
            //customKeyValueFilter.ApplicationProperties["Day"] = "Saturday";

            CreateRuleOptions createRuleOptions = new CreateRuleOptions
            {
                Name = "DaysFilter",
                Filter = new SqlRuleFilter("Day In ( 'Saturday', 'Sunday')") //customKeyValueFilter
            };
            await _adminClient.CreateRuleAsync(Constants.TopicName, subscriptionName, createRuleOptions);
        }

        private async Task AddFilterToWeekday(string subscriptionName, List<RuleProperties> ruleProperties)
        {
            //var customKeyValueFilter = new CorrelationRuleFilter();
            //customKeyValueFilter.ApplicationProperties["Day"] = "Saturday";

            CreateRuleOptions createRuleOptions = new CreateRuleOptions
            {
                Name = "DaysFilter",
                Filter = new SqlRuleFilter("Day Not In ( 'Saturday', 'Sunday')") //customKeyValueFilter
            };
            await _adminClient.CreateRuleAsync(Constants.TopicName, subscriptionName, createRuleOptions);
        }

        private async Task RemoveDefaultFilters(string subscriptionName, List<RuleProperties> ruleProperties)
        {
            try
            {
                foreach (var rule in ruleProperties) //CreateRuleOptions.DefaultRuleName
                {
                    await _adminClient.DeleteRuleAsync(Constants.TopicName, subscriptionName, rule.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task WeekendSubscriptionMessagesAsync(ProcessMessageEventArgs arg)
        {
            var @event = arg.Message.Body.ToObjectFromJson<TodoTaskCreatedEvent>();
            var handlerTypes = _eventBusSubscriptionsManager.GetHandlersForEvent(@event.EventName, SubscriptionNames.weekend.ToString());
            foreach (var handlerType in handlerTypes)
            {
                dynamic handlerInstance = _serviceProvider.GetService(handlerType);
                // http://techxposer.com/2018/01/03/missing-compiler-required-member-microsoft-csharp-runtimebinder-csharpargumentinfo-create-solved/
                // https://social.msdn.microsoft.com/Forums/sharepoint/en-US/2b855369-a721-4010-9e33-72d699960994/how-to-fix-missing-compiler-member-error-microsoftcsharpruntimebindercsharpargumentinfocreate?forum=visualstudiogeneral
                await handlerInstance?.HandleAsync(@event);
            }
            await arg.CompleteMessageAsync(arg.Message);
        }

        private async Task WeekdaySubscriptionMessagesAsync(ProcessMessageEventArgs arg)
        {
            var @event = arg.Message.Body.ToObjectFromJson<TodoTaskCreatedEvent>();
            var handlerTypes = _eventBusSubscriptionsManager.GetHandlersForEvent(@event.EventName, SubscriptionNames.weekday.ToString());
            foreach (var handlerType in handlerTypes)
            {
                dynamic handlerInstance = _serviceProvider.GetService(handlerType);
                // http://techxposer.com/2018/01/03/missing-compiler-required-member-microsoft-csharp-runtimebinder-csharpargumentinfo-create-solved/
                // https://social.msdn.microsoft.com/Forums/sharepoint/en-US/2b855369-a721-4010-9e33-72d699960994/how-to-fix-missing-compiler-member-error-microsoftcsharpruntimebindercsharpargumentinfocreate?forum=visualstudiogeneral
                await handlerInstance?.HandleAsync(@event);
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
