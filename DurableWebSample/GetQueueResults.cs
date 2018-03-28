using System;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json; 

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
                ResponseMsg msg = JsonConvert.DeserializeObject<ResponseMsg>(myQueueItem);
                await client.RaiseEventAsync(msg.BucketID, "GotMsg", myQueueItem);
                log.Info($"Message Id processed: {msg.BucketID}");
            }
            catch (Exception err)
            {
                log.Info(err.Message);
            }
            return;
        }
    }
    public class ResponseMsg
    {
        public string BucketID { get; set; }
        public string Response { get; set; }

    }
}
