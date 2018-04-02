using System;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DurableWebSample
{
    public static class GetQueueResults
    {
        [FunctionName("GetQueueResults")]

        public static async void Run([QueueTrigger("out-data", Connection = "function0c5fcf20a642_STORAGE")]string myQueueItem, [OrchestrationClient] DurableOrchestrationClient client, TraceWriter log)
        {
            log.Info("queueu running: " + myQueueItem);
            try
            {
                //assumes there is an ID in the message, rest is specific
                JObject msg = JObject.Parse(myQueueItem);
                await client.RaiseEventAsync((string)msg["Id"], "GotMsg", myQueueItem);
                log.Info($"Message Id processed: {myQueueItem}");
            }
            catch (Exception err)
            {
                log.Info(err.Message);
            }
            return;
        }
    }
}
