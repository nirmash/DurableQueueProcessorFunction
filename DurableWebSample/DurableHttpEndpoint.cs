using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace DurableWebSample
{
    public static class DurableHttpEndpoint
    {
        [FunctionName("DurableHttpEndpoint")]
        public static async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
        [OrchestrationClient] DurableOrchestrationClient starter,
        TraceWriter log)
        {
            string strContent = await req.Content.ReadAsStringAsync();
            string instanceId = await starter.StartNewAsync("QueueAsyncOrchestrator", strContent);
            log.Info($"Started orchestration with ID = '{instanceId}'.");
            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
