using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PassionLib.DAL;
using PassionLib.Models;
namespace MeltBot.Modules
{
    internal class AddCommands : BaseCommandModule
    {
        public RunsContext Context { private get; set; }
        [Command("AddServant")]
        [Description("Add an existing servant to the database.")]
        public async Task AddServant(CommandContext ctx,
            [Description("Id or collectionNo of the servant to add")] int servantId,
            [Description("(Optional)A nickname to that servant.")] string nickname = null)
        {
            string str = string.Empty;
            try
            {
                Servant? s = Context.Servants.FirstOrDefault(s => s.Id == servantId || s.CollectionNo == servantId);
                if (s is null)
                {
                    using (var client = new HttpClient())
                    {
                        JObject? j = JsonConvert.DeserializeObject<JObject>(await client.GetStringAsync($"https://api.atlasacademy.io/nice/JP/servant/{servantId}"));
                        if (j is null) throw new Exception("Problem with " + servantId);
                        string n = j.Value<string>("name");
                        s = new Servant(j.Value<int>("id"), n, j.Value<int>("collectionNo"))
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

                        Context.Servants.Add(s);
                        Context.SaveChanges();
                        str = $"Successfully added {n}.";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }

            await ctx.Channel.SendMessageAsync(str);
            if (nickname is not null) await AddServantNickname(ctx, servantId.ToString(), nickname);
        }
        [Command("NickS")]
        [Description("Add a nickname to a servant")]
        public async Task AddServantNickname(CommandContext ctx,
            [Description("Id or collectionNo or nickname of servant")] string servant,
            [Description("New nickname")] string nickname)
        {
            string str = string.Empty;
            try
            {
                Servant? s;
                if (int.TryParse(servant, out int id))
                {
                    s = Context.Servants.FirstOrDefault(s => s.Id == id || s.CollectionNo == id);
                    if (s is null) throw new Exception($"{servant} could not be found.");
                }
                else
                {
                    s = Context.ServantAliases.FirstOrDefault(x => x.Nickname == servant)?.Servant;
                    if (s is null) throw new Exception($"{servant} could not be found.");
                }
                User u = DbHelper.GetUser(ctx, Context);
                Context.ServantAliases.Add(new ServantAlias(s, nickname) { Submitter = u });
                Context.SaveChanges();
                str = $"Nickname {nickname} added for servant {servant}.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
        }
        [Command("AddQuest")]
        [Description("Add an existing quest to the database.")]
        public async Task AddQuest(CommandContext ctx,
             [Description("Id of the quest to add")] int questId,
             [Description("(Optional)A nickname to that quest.")] string nickname = null)
        {
            string str = string.Empty;
            try
            {
                Quest? q = Context.Quests.FirstOrDefault(s => s.Id == questId);
                if (q is null)
                {
                    using (var client = new HttpClient())
                    {
                        JObject? j = JsonConvert.DeserializeObject<JObject>(await client.GetStringAsync($"https://api.atlasacademy.io/basic/JP/quest/{questId}"));
                        if (j is null) throw new Exception("Problem with " + questId);
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
                        Context.Quests.Add(q);
                        Context.SaveChanges();
                        str = $"Successfully added {n}.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
            if (nickname is not null) await AddServantNickname(ctx, questId.ToString(), nickname);
        }
        [Command("NickQ")]
        [Description("Add a nickname to a quest")]
        public async Task AddQuestNickname(CommandContext ctx,
            [Description("Id or nickname of quest")] string quest,
            [Description("New nickname")] string nickname)
        {
            string str = string.Empty;
            try
            {
                Quest? q;
                if (int.TryParse(quest, out int id))
                {
                    q = Context.Quests.FirstOrDefault(q => q.Id == id);
                    if (q is null) throw new Exception($"{quest} could not be found.");
                }
                else
                {
                    q = Context.QuestAliases.FirstOrDefault(x => x.Nickname == quest)?.Quest;
                    if (q is null) throw new Exception($"{quest} could not be found.");
                }
                User u = DbHelper.GetUser(ctx, Context);
                Context.QuestAliases.Add(new QuestAlias(q, nickname) { Submitter = u });
                Context.SaveChanges();
                str = $"Nickname {nickname} added for quest {quest}.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
        }
        [Command("AddCe")]
        [Description("Add an existing craft essence to the database.")]
        public async Task AddCe(CommandContext ctx,
             [Description("Id or collectionNo of the ce to add")] int ceId,
             [Description("(Optional)A nickname to that ce.")] string nickname = null)
        {
            string str = string.Empty;
            try
            {
                CraftEssence? ce = Context.CraftEssences.FirstOrDefault(s => s.Id == ceId || s.CollectionNo == ceId);
                if (ce is null)
                {
                    using (var client = new HttpClient())
                    {
                        JObject? j = JsonConvert.DeserializeObject<JObject>(await client.GetStringAsync($"https://api.atlasacademy.io/nice/JP/equip/{ceId}"));
                        if (j is null) throw new Exception("Problem with " + ceId);
                        string n = j.Value<string>("name");
                        ce = new CraftEssence(j.Value<int>("id"), n, j.Value<int>("collectionNo"))
                        {
                            BaseMaxAttack = j.Value<short>("atkMax"),
                            AttackScaling = j.GetValue("atkGrowth").ToObject<short[]>(),
                            Rarity = j.Value<short>("rarity")
                        };
                        Context.CraftEssences.Add(ce);
                        Context.SaveChanges();
                        str = $"Successfully added {n}.";

                        var response = await client.GetAsync($"https://api.atlasacademy.io/basic/NA/equip/{ceId}");
                        if (response.IsSuccessStatusCode)
                        {
                            j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                            ce.NaName = j?.Value<string>("name");
                        }
                        response = await client.GetAsync($"https://api.atlasacademy.io/basic/CN/equip/{ceId}");
                        if (response.IsSuccessStatusCode)
                        {
                            j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                            ce.CnName = j?.Value<string>("name");
                        }
                        response = await client.GetAsync($"https://api.atlasacademy.io/basic/KR/equip/{ceId}");
                        if (response.IsSuccessStatusCode)
                        {
                            j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                            ce.KrName = j?.Value<string>("name");
                        }
                        response = await client.GetAsync($"https://api.atlasacademy.io/basic/TW/equip/{ceId}");
                        if (response.IsSuccessStatusCode)
                        {
                            j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                            ce.TwName = j?.Value<string>("name");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
            if (nickname is not null) await AddCeNickname(ctx, ceId.ToString(), nickname);
        }
        [Command("NickCe")]
        [Description("Add a nickname to a craft essence")]
        public async Task AddCeNickname(CommandContext ctx,
            [Description("Id or nickname of craft essence")] string ce,
            [Description("New nickname")] string nickname)
        {
            string str = string.Empty;
            try
            {
                CraftEssence? c;
                if (int.TryParse(ce, out int id))
                {
                    c = Context.CraftEssences.FirstOrDefault(q => q.Id == id);
                    if (c is null) throw new Exception($"{ce} could not be found.");
                }
                else
                {
                    c = Context.CraftEssenceAliases.FirstOrDefault(x => x.Nickname == ce)?.CraftEssence;
                    if (c is null) throw new Exception($"{ce} could not be found.");
                }
                User u = DbHelper.GetUser(ctx, Context);
                Context.CraftEssenceAliases.Add(new CraftEssenceAlias(c, nickname) { Submitter = u });
                Context.SaveChanges();
                str = $"Nickname {nickname} added for craft essence {ce}.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
        }
        [Command("AddMc")]
        [Description("Add an existing mystic quest to the database.")]
        public async Task AddMc(CommandContext ctx,
             [Description("Id of the mystic code to add")] int mcId,
             [Description("(Optional)A nickname to that mc.")] string nickname = null)
        {
            string str = string.Empty;
            try
            {
                MysticCode? mc = Context.MysticCodes.FirstOrDefault(s => s.Id == mcId);
                if (mc is null)
                {
                    using (var client = new HttpClient())
                    {
                        JObject? j = JsonConvert.DeserializeObject<JObject>(await client.GetStringAsync($"https://api.atlasacademy.io/basic/JP/MC/{mcId}"));
                        if (j is null) throw new Exception("Problem with " + mcId);
                        string n = j.Value<string>("name");
                        mc = new MysticCode(j.Value<int>("id"), n);
                        

                        var response = await client.GetAsync($"https://api.atlasacademy.io/basic/NA/MC/{mcId}");
                        if (response.IsSuccessStatusCode)
                        {
                            j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                            mc.NaName = j?.Value<string>("name");
                        }
                        response = await client.GetAsync($"https://api.atlasacademy.io/basic/CN/MC/{mcId}");
                        if (response.IsSuccessStatusCode)
                        {
                            j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                            mc.CnName = j?.Value<string>("name");
                        }
                        response = await client.GetAsync($"https://api.atlasacademy.io/basic/KR/MC/{mcId}");
                        if (response.IsSuccessStatusCode)
                        {
                            j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                            mc.KrName = j?.Value<string>("name");
                        }
                        response = await client.GetAsync($"https://api.atlasacademy.io/basic/TW/servant/{mcId}");
                        if (response.IsSuccessStatusCode)
                        {
                            j = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                            mc.TwName = j?.Value<string>("name");
                        }
                        Context.MysticCodes.Add(mc);
                        Context.SaveChanges();
                        str = $"Successfully added {n}.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
            if (nickname is not null) await AddMcNickname(ctx, mcId.ToString(), nickname);
        }
        [Command("NickMc")]
        [Description("Add a nickname to a mystic code")]
        public async Task AddMcNickname(CommandContext ctx,
            [Description("Id or nickname of mystic code")] string mc,
            [Description("New nickname")] string nickname)
        {
            string str = string.Empty;
            try
            {
                MysticCode? m;
                if (int.TryParse(mc, out int id))
                {
                    m = Context.MysticCodes.FirstOrDefault(q => q.Id == id);
                    if (m is null) throw new Exception($"{mc} could not be found.");
                }
                else
                {
                    m = Context.MysticCodeAliases.FirstOrDefault(x => x.Nickname == mc)?.MysticCode;
                    if (m is null) throw new Exception($"{mc} could not be found.");
                }
                User u = DbHelper.GetUser(ctx, Context);
                Context.MysticCodeAliases.Add(new MysticCodeAlias(m, nickname) { Submitter = u });
                Context.SaveChanges();
                str = $"Nickname {nickname} added for mystic code {mc}.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
        }
    }
}
