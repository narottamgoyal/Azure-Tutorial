using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
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
        private readonly IList<string> names;

        public HostedBackgroundService(IEventConsumerService serviceBusConsumer, IList<string> names)
        {
            _serviceBusConsumer = serviceBusConsumer;
            this.names = names;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _serviceBusConsumer.RegisterBaseEventHandlerAsync(names);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serviceBusConsumer.StopListenerAsync();
        }
    }
}
