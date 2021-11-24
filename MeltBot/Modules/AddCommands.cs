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
                User u = Commands.GetUser(ctx, Context);
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
                User u = Commands.GetUser(ctx, Context);
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
        [Description("Add an existing ce to the database.")]
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
                        JObject? j = JsonConvert.DeserializeObject<JObject>(await client.GetStringAsync($"https://api.atlasacademy.io/nice/JP/servant/{ceId}"));
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
        [Description("Add a nickname to a quest")]
        public async Task AddCeNickname(CommandContext ctx,
            [Description("Id or nickname of ce")] string ce,
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
                User u = Commands.GetUser(ctx, Context);
                Context.CraftEssenceAliases.Add(new CraftEssenceAlias(c, nickname) { Submitter = u });
                Context.SaveChanges();
                str = $"Nickname {nickname} added for quest {ce}.";
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
