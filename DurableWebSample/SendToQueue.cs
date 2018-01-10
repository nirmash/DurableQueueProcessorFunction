using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableWebSample
{
    [StorageAccount("function0c5fcf20a642_STORAGE")]
    public static class SendToQueue
    {
        [FunctionName("SendToQueue")]
        [return: Queue("in-data")]
        public static string Run([ActivityTrigger] string msg, TraceWriter log)
        {
            log.Info($"Send message to Queue = '{msg}'.");
            return msg;
        }
    }
}
