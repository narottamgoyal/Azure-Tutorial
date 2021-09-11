using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace AzureEventServiceBus
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/master/src/Services/Ordering/Ordering.BackgroundTasks/Tasks/GracePeriodManagerTask.cs
    /// </summary>
    public class HostedBackgroundService : IHostedService
    {
        private readonly IEventConsumerService _serviceBusConsumer;

        public HostedBackgroundService(IEventConsumerService serviceBusConsumer)
        {
            _serviceBusConsumer = serviceBusConsumer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _serviceBusConsumer.RegisterWeekendSubscriptionAsync();
            await _serviceBusConsumer.RegisterWeekdaySubscriptionAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serviceBusConsumer.UnSubscribeAsync();
        }
    }
}
