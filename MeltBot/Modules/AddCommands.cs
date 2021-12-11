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
    [ModuleLifespan(ModuleLifespan.Transient)]
    internal class AddCommands : BaseCommandModule
    {
        public RunsContext Context { private get; set; }

        [Hidden]
        [Command("Update")]
        public async Task Update(CommandContext ctx)
        {
            if (Bot.Admin.ContainsKey(ctx.User.Id))
            {
                foreach (var ce in Context.CraftEssences)
                {
                    await AddCe(ctx, ce.Id);
                }
                //foreach (var s in Context.Servants)
                //{
                //    await AddServant(ctx, s.Id);
                //}
                //foreach(var q in Context.Quests)
                //{
                //    await AddQuest(ctx, q.Id);
                //}
                //foreach(var mc in Context.MysticCodes)
                //{
                //    await AddMc(ctx, mc.Id);
                //}
                await ctx.Channel.SendMessageAsync("Done updating");
            }
        }

        [Command("AddServant")]
        [Description("Add an existing servant to the database")]
        public async Task AddServant(CommandContext ctx,
            [Description("Id or collectionNo of the servant to add")] int servantId,
            [Description("(Optional)A nickname to that servant")] string nickname = null)
        {
            string str = string.Empty;
            try
            {
                Servant? s = Context.Servants.FirstOrDefault(s => s.Id == servantId || s.CollectionNo == servantId);
                bool flag = s is null;

                s = await ServantData(servantId, s);

                if (flag) Context.Servants.Add(s);

                await Context.SaveChangesAsync();
                str = $"Successfully added {s.JpName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }

            await ctx.Channel.SendMessageAsync(str);
            if (nickname is not null) await AddServantNickname(ctx, servantId.ToString(), nickname);
        }

        private static async Task<Servant> ServantData(int servantId, Servant? s)
        {
            using (var client = new HttpClient())
            {
                JObject? j = JsonConvert.DeserializeObject<JObject>(await client.GetStringAsync($"https://api.atlasacademy.io/nice/JP/servant/{servantId}"));
                if (j is null) throw new Exception("Problem with " + servantId);
                string n = j.Value<string>("name");
                s = s is null ? new Servant(j.Value<int>("id"), n, j.Value<int>("collectionNo")) : s;

                s.Cost = j.Value<short>("cost");
                s.BaseMaxAttack = j.Value<short>("atkMax");
                s.AttackScaling = j.GetValue("atkGrowth").ToObject<short[]>();
                s.Class = j.Value<string>("className");
                s.Rarity = j.Value<short>("rarity");

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
            }
            return s;
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
                if (Context.ServantAliases.Any(x => x.Nickname == nickname)) throw new Exception($"nickname {nickname} already exists");
                Servant? s;
                if (int.TryParse(servant, out int id))
                {
                    s = Context.Servants.FirstOrDefault(s => s.Id == id || s.CollectionNo == id);
                    if (s is null) throw new Exception($"{servant} could not be found");
                }
                else
                {
                    s = Context.ServantAliases.FirstOrDefault(x => x.Nickname == servant)?.Servant;
                    if (s is null) throw new Exception($"{servant} could not be found");
                }
                User u = DbHelper.GetUser(ctx, Context);
                Context.ServantAliases.Add(new ServantAlias(s, nickname) { Submitter = u });
                Context.SaveChanges();
                str = $"Nickname {nickname} added for servant {servant}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
        }
        [Command("AddQuest")]
        [Description("Add an existing quest to the database")]
        public async Task AddQuest(CommandContext ctx,
             [Description("Id of the quest to add")] int questId,
             [Description("(Optional)A nickname to that quest")] string nickname = null)
        {
            string str = string.Empty;
            try
            {
                Quest? q = Context.Quests.FirstOrDefault(s => s.Id == questId);
                bool flag = q is null;

                q = await QuestData(questId, q);
                if (flag) Context.Quests.Add(q);
                Context.SaveChanges();
                str = $"Successfully added {q.JpName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
            if (nickname is not null) await AddQuestNickname(ctx, questId.ToString(), nickname);
        }

        private static async Task<Quest> QuestData(int questId, Quest? q)
        {
            using (var client = new HttpClient())
            {
                JObject? j = JsonConvert.DeserializeObject<JObject>(await client.GetStringAsync($"https://api.atlasacademy.io/basic/JP/quest/{questId}"));
                if (j is null) throw new Exception("Problem with " + questId);
                string n = j.Value<string>("name");
                q = q is null ? new Quest(j.Value<int>("id"), n) : q;
                DateTimeOffset d = DateTimeOffset.FromUnixTimeSeconds(j.Value<int>("openedAt"));
                q.CreatedDate = d.UtcDateTime;
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

            return q;
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
                if (Context.QuestAliases.Any(x => x.Nickname == nickname)) throw new Exception($"nickname {nickname} already exists");
                Quest? q;
                if (int.TryParse(quest, out int id))
                {
                    q = Context.Quests.FirstOrDefault(q => q.Id == id);
                    if (q is null) throw new Exception($"{quest} could not be found");
                }
                else
                {
                    q = Context.QuestAliases.FirstOrDefault(x => x.Nickname == quest)?.Quest;
                    if (q is null) throw new Exception($"{quest} could not be found");
                }
                User u = DbHelper.GetUser(ctx, Context);
                Context.QuestAliases.Add(new QuestAlias(q, nickname) { Submitter = u });
                Context.SaveChanges();
                str = $"Nickname {nickname} added for quest {quest}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
        }
        [Command("AddCe")]
        [Description("Add an existing craft essence to the database")]
        public async Task AddCe(CommandContext ctx,
             [Description("Id or collectionNo of the ce to add")] int ceId,
             [Description("(Optional)A nickname to that ce")] string nickname = null)
        {
            string str = string.Empty;
            try
            {
                CraftEssence? ce = Context.CraftEssences.FirstOrDefault(s => s.Id == ceId || s.CollectionNo == ceId);
                bool flag = ce is null;
                ce = await CeData(ceId, ce);
                if (flag) Context.CraftEssences.Add(ce);
                await Context.SaveChangesAsync();
                str = $"Successfully added {ce.JpName}";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
            if (nickname is not null) await AddCeNickname(ctx, ceId.ToString(), nickname);


        }

        private static async Task<CraftEssence> CeData(int ceId, CraftEssence? ce)
        {
            using (var client = new HttpClient())
            {
                JObject? j = JsonConvert.DeserializeObject<JObject>(await client.GetStringAsync($"https://api.atlasacademy.io/nice/JP/equip/{ceId}"));
                if (j is null) throw new Exception("Problem with " + ceId);
                string n = j.Value<string>("name");
                ce = ce is null ? new CraftEssence(j.Value<int>("id"), n, j.Value<int>("collectionNo")) : ce;

                ce.Cost = j.Value<short>("cost");
                ce.BaseMaxAttack = j.Value<short>("atkMax");
                ce.AttackScaling = j.GetValue("atkGrowth").ToObject<short[]>();
                ce.Rarity = j.Value<short>("rarity");


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

            return ce;
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
                if (Context.CraftEssenceAliases.Any(x => x.Nickname == nickname)) throw new Exception($"nickname {nickname} already exists");
                CraftEssence? c;
                if (int.TryParse(ce, out int id))
                {
                    c = Context.CraftEssences.FirstOrDefault(q => q.Id == id);
                    if (c is null) throw new Exception($"{ce} could not be found");
                }
                else
                {
                    c = Context.CraftEssenceAliases.FirstOrDefault(x => x.Nickname == ce)?.CraftEssence;
                    if (c is null) throw new Exception($"{ce} could not be found");
                }
                User u = DbHelper.GetUser(ctx, Context);
                Context.CraftEssenceAliases.Add(new CraftEssenceAlias(c, nickname) { Submitter = u });
                await Context.SaveChangesAsync();
                str = $"Nickname {nickname} added for craft essence {ce}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
        }
        [Command("AddMc")]
        [Description("Add an existing mystic quest to the database")]
        public async Task AddMc(CommandContext ctx,
             [Description("Id of the mystic code to add")] int mcId,
             [Description("(Optional)A nickname to that mc")] string nickname = null)
        {
            string str = string.Empty;
            try
            {
                MysticCode? mc = Context.MysticCodes.FirstOrDefault(s => s.Id == mcId);
                bool flag = mc is null;
                mc = await McData(mcId, mc);
                if (flag) Context.MysticCodes.Add(mc);
                await Context.SaveChangesAsync();
                str = $"Successfully added {mc.JpName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                str = ex.Message;
            }
            await ctx.Channel.SendMessageAsync(str);
            if (nickname is not null) await AddMcNickname(ctx, mcId.ToString(), nickname);
        }

        private static async Task<MysticCode> McData(int mcId, MysticCode? mc)
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
            }
            return mc;
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
                if (Context.MysticCodeAliases.Any(x => x.Nickname == nickname)) throw new Exception($"nickname {nickname} already exists");
                MysticCode? m;
                if (int.TryParse(mc, out int id))
                {
                    m = Context.MysticCodes.FirstOrDefault(q => q.Id == id);
                    if (m is null) throw new Exception($"{mc} could not be found");
                }
                else
                {
                    m = Context.MysticCodeAliases.FirstOrDefault(x => x.Nickname == mc)?.MysticCode;
                    if (m is null) throw new Exception($"{mc} could not be found");
                }
                User u = DbHelper.GetUser(ctx, Context);
                Context.MysticCodeAliases.Add(new MysticCodeAlias(m, nickname) { Submitter = u });
                Context.SaveChanges();
                str = $"Nickname {nickname} added for mystic code {mc}";
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
