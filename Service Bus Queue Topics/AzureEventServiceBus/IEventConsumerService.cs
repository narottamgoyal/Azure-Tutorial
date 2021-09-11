using System.Threading.Tasks;

namespace AzureEventServiceBus
{
    public interface IEventConsumerService
    {
        Task RegisterWeekendSubscriptionAsync();
        Task RegisterWeekdaySubscriptionAsync();
        Task UnSubscribeAsync();
    }
}