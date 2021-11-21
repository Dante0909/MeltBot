using PassionLib.Models;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlazorOfLimbo.Client.Service
{
    public class RunService : IRunService
    {
        private readonly HttpClient client;

        public RunService(HttpClient client)
        {
            this.client = client;
        }
        public async Task<Run> GetRuns()
        {
            Run r;
            using (var client = new HttpClient())
            {
                var message = await client.GetStringAsync("https://localhost:7209/api/runs").ConfigureAwait(false);
                
                r = JsonConvert.DeserializeObject<Run>(message);
                JObject j;
                //j["Quest"]["Id"]
             }
            return r;
            //return await client.GetFromJsonAsync<List<Run>>("https://localhost:7209/api/runs");
            
        }

    }
}
