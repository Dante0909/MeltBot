using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PassionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.DAL
{
    public static class RunDbInitializer
    {
        public static async Task Initialize(RunsContext context)
        {
            if (context.Runs.Any())
            {
                Console.WriteLine("herre");
                return; // DB already has data
            }
            var woahnilandRerunCq = context.Quests.FirstOrDefault(o => o.Id == 94042801);
            bool isOnline = CheckForInternetConnection();


            if (woahnilandRerunCq == null)
            {
                if (isOnline)
                {
                    string str = string.Empty;
                    try
                    {
                        Quest? q = context.Quests.FirstOrDefault(s => s.Id == 94042801);
                        if (q is null)
                        {
                            using (var client = new HttpClient())
                            {
                                var questId = 94042801;
                                JObject? j = JsonConvert.DeserializeObject<JObject>(await client.GetStringAsync($"https://api.atlasacademy.io/basic/JP/quest/{questId}"));
                                if (j is null) throw new Exception("Problem with " + 94042801);
                                string n = j.Value<string>("name");
                                q = new Quest(j.Value<int>("id"), n);

                                var response = await client.GetAsync($"https://api.atlasacademy.io/basic/NA/quest/{questId}");
                                if (response.IsSuccessStatusCode)
                                {
                                    j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                                    q.NaName = j?.Value<string>("name");
                                }
                                response = await client.GetAsync($"https://api.atlasacademy.io/basic/CN/quest/{questId}");
                                if (response.IsSuccessStatusCode)
                                {
                                    j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                                    q.CnName = j?.Value<string>("name");
                                }
                                response = await client.GetAsync($"https://api.atlasacademy.io/basic/KR/quest/{questId}");
                                if (response.IsSuccessStatusCode)
                                {
                                    j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                                    q.KrName = j?.Value<string>("name");
                                }
                                response = await client.GetAsync($"https://api.atlasacademy.io/basic/TW/quest/{questId}");
                                if (response.IsSuccessStatusCode)
                                {
                                    j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                                    q.TwName = j?.Value<string>("name");
                                }
                            }
                        }
                        woahnilandRerunCq = q;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        str = ex.Message;
                    }
                }
                else
                {
                    woahnilandRerunCq = new Quest(94042801,
                                                  "【高難易度】護法少女スペシャルヒーローショー");
                    woahnilandRerunCq.NaName = "[High Difficulty] Magifender Girls Special Hero Show";
                }


                context.Quests.Add(woahnilandRerunCq);
            }
            //var imaginaryScramble = context.Quests.FirstOrDefault(q => q.Id == 94053435);
            //if (imaginaryScramble is null)
            //{
            //    imaginaryScramble = new Quest(94053435, "【高難易度】聖女を呼ぶ声");
            //    context.Quests.Add(imaginaryScramble);
            //}
            Servant skadi = null;
            Servant charlotte = null;
            Servant lanling = null;
            Servant tamamo = null;
            Servant bride = null;
            if (isOnline)
            {
                try
                {
                    Servant? s = context.Servants.FirstOrDefault(s => s.Id == 603800);
                    if (s is null) charlotte = await AddServant(context, 603800);
                    s = context.Servants.FirstOrDefault(s => s.Id == 503900);
                    if (s is null) skadi = await AddServant(context, 503900);
                    s = context.Servants.FirstOrDefault(s => s.Id == 103600);
                    if (s is null) lanling = await AddServant(context, 103600);
                    s = context.Servants.FirstOrDefault(s => s.Id == 500300);
                    if (s is null) tamamo = await AddServant(context, 500300);
                    s = context.Servants.FirstOrDefault(s => s.Id == 100600);
                    if (s is null) bride = await AddServant(context, 100600);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                return;
                //charlotte = new Servant(603800, "シャルロット・コルデー", 1);
            }


            context.Servants.AddRange(skadi, charlotte, lanling, tamamo, bride);
            //User u = new User();
            //var runs = new Run[]
            //{
            //    new Run(woahnilandRerunCq,"https://youtu.be/xqu9_kDYPvo",charlotte, u)
            //    {
            //        Party = new List<PartySlot>()
            //        {
            //            new PartySlot()
            //            {


            //            }
            //        }
            //    }
            ////new Run(woahnilandRerunCq, "https://youtu.be/JGo6nnMMu7g"),
            ////new Run(woahnilandRerunCq, "https://youtu.be/gwwTwJ1F9WY"),
            ////new Run(woahnilandRerunCq, "https://youtu.be/7ZrAoqPJ4qw")
            //};
            //context.Users.Add(u);
            //context.Runs.AddRange(runs);

            context.SaveChanges();
        }
        public static async Task<Servant> AddServant(RunsContext context, int servantId)
        {
            using (var client = new HttpClient())
            {
                JObject? j = JsonConvert.DeserializeObject<JObject>(await client.GetStringAsync($"https://api.atlasacademy.io/nice/JP/servant/{servantId}"));
                if (j is null) throw new Exception("Problem with " + servantId);
                string n = j.Value<string>("name");
                Servant s = new Servant(j.Value<int>("id"), n, j.Value<int>("collectionNo"))
                {
                    BaseMaxAttack = j.Value<short>("atkMax"),
                    AttackScaling = j.GetValue("atkGrowth").ToObject<short[]>(),
                    Class = j.Value<string>("className"),
                    Rarity = j.Value<short>("rarity")
                };
                var response = await client.GetAsync($"https://api.atlasacademy.io/basic/NA/servant/{servantId}");
                if (response.IsSuccessStatusCode)
                {
                    j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                    s.NaName = j?.Value<string>("name");
                }
                response = await client.GetAsync($"https://api.atlasacademy.io/basic/CN/servant/{servantId}");
                if (response.IsSuccessStatusCode)
                {
                    j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                    s.CnName = j?.Value<string>("name");
                }
                response = await client.GetAsync($"https://api.atlasacademy.io/basic/KR/servant/{servantId}");
                if (response.IsSuccessStatusCode)
                {
                    j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                    s.KrName = j?.Value<string>("name");
                }
                response = await client.GetAsync($"https://api.atlasacademy.io/basic/TW/servant/{servantId}");
                if (response.IsSuccessStatusCode)
                {
                    j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                    s.TwName = j?.Value<string>("name");
                }
                return s;
            }
        }
        public static bool CheckForInternetConnection(int timeoutMs = 10000)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://www.gstatic.com/generate_204");
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using (var response = (HttpWebResponse)request.GetResponse())
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
