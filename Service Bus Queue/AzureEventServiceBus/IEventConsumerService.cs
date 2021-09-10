using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureEventServiceBus
{
    public interface IEventConsumerService
    {
        Task RegisterBaseEventHandlerAsync(IList<string> names);
        Task StopListenerAsync();
    }
}