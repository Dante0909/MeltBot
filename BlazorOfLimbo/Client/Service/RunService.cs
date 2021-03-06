using PassionLib.Models;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlazorOfLimbo.Client.Service
{
    public class RunService : IRunService
    {
        private readonly HttpClient client;
        private List<Run> runs;

        public RunService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<List<Quest>> GetQuests()
        {
            var str = await client.GetStringAsync("api/quests");
            List<Quest> quests = JsonConvert.DeserializeObject<List<Quest>>(str);
            return quests;
        }

        public async Task<List<Run>> GetRuns()
        {
            if(runs is null || runs.Count == 0)
            {
                var str = await client.GetStringAsync("api/runs");
                runs = JsonConvert.DeserializeObject<List<Run>>(str);
            }
            return runs;
            //Run r;
            //using (var client = new HttpClient())
            //{
            //    var message = await client.GetStringAsync("https://localhost:7209/api/runs").ConfigureAwait(false);

            //    r = JsonConvert.DeserializeObject<Run>(message);
            //    JObject j;
            //    //j["Quest"]["Id"]
            // }
            


        }

    }
}
