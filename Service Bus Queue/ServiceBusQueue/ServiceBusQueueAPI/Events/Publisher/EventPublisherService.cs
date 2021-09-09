using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceBusQueueAPI.Events.Publisher
{
    public interface IEventPublisherService
    {
        Task PublishMessageAsync<T>(T eventMessage);
    }

    public class EventPublisherService : IEventPublisherService
    {
        private readonly string ConnectionString = "Endpoint=sb://ngtestsb2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=6qE0gKKXeLOlGCmC/T1YfcwIvd2/nnHxt3Kj7diEGkQ=";
        public async Task PublishMessageAsync<T>(T eventMessage)
        {
            var queueClient = new QueueClient(ConnectionString, eventMessage.GetType().Name);
            string messageBody = JsonSerializer.Serialize(eventMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await queueClient.SendAsync(message);
        }
    }
}
