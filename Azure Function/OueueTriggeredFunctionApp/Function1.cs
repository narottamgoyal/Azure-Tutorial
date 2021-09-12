using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OueueTriggeredFunctionApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        [return: ServiceBus("servicebusqueueapi.events.sampledemo2event", Connection = "ServiceBusConnection")]
        public static string Run([ServiceBusTrigger("servicebusqueueapi.events.sampledemo1event", Connection = "ServiceBusConnection")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            
            dynamic data = JsonConvert.DeserializeObject(myQueueItem);
            data.Message = "From Azure Function:" + data?.Message;
            data.EventName = "SampleDemo2Event";
            
            return JsonConvert.SerializeObject(data);
        }
    }
}
