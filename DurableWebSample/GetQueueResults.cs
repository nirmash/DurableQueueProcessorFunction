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
                PiMsg msg = JsonConvert.DeserializeObject<PiMsg>(myQueueItem);
                log.Info(msg.Temp.ToString());
                await client.RaiseEventAsync(msg.Id, "GotMsg", myQueueItem);
                log.Info($"Message Id processed: {msg.Id}");
            }
            catch (Exception err)
            {
                log.Info(err.Message);
            }
            return;
        }
    }
    public class PiMsg
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public string Id { get; set; }
    }
}
