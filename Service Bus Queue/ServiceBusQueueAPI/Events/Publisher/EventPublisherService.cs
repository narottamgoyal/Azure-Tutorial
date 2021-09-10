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
        private readonly string ConnectionString = "";
        public async Task PublishMessageAsync<T>(T eventMessage)
        {
            var queueClient = new QueueClient(ConnectionString, typeof(T).FullName);
            string messageBody = JsonSerializer.Serialize(eventMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await queueClient.SendAsync(message);
        }
    }
}
