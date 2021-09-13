using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceBusQueueAPI.Events
{
    public interface IEventBusService
    {
        Task Publish<T>(T eventMessage);
    }

    public class EventBusService : IEventBusService
    {
        private readonly string ConnectionString;

        public EventBusService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task Publish<T>(T eventMessage)
        {
            var queueClient = new QueueClient(ConnectionString, typeof(T).FullName);
            string messageBody = JsonSerializer.Serialize(eventMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            await queueClient.SendAsync(message);
        }
    }
}
