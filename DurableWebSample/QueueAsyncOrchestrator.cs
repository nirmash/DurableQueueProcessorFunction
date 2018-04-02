using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace DurableWebSample
{
    public static class QueueAsyncOrchestrator
    {
        [FunctionName("QueueAsyncOrchestrator")]
        public static async Task<object> Run([OrchestrationTrigger] DurableOrchestrationContext context, TraceWriter log)
        {
            var queueTask = await context.CallActivityAsync<Task>("SendToQueue", context.InstanceId);

            TimeSpan timeout = TimeSpan.FromSeconds(4);
            DateTime deadline = context.CurrentUtcDateTime.Add(timeout);
            log.Info(deadline.ToString());
            using (var cts = new CancellationTokenSource())
            {

                Task<string> activityTask = context.WaitForExternalEvent<string>("GotMsg");
                Task timeoutTask = context.CreateTimer(deadline, cts.Token);

                Task winner = await Task.WhenAny(activityTask, timeoutTask);
                if (winner == activityTask)
                {
                    // success case
                    cts.Cancel();
                    log.Info(activityTask.Result);
                    return activityTask.Result;
                }
                else
                {
                    log.Info("timeout case");
                    // timeout case 
                    return "Timed Out";
                }
            }
        }
    }
}
