using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using PassionLib.DAL;
using PassionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeltBot.Modules
{
    internal class GetCommands : BaseCommandModule
    {
        public RunsContext Context { private get; set; }
        public DiscordChannel DebugChannel { private get; set; }

        [Command("GetSvtNames")]
        [Description("Get nicknames of a given servant")]
        public async Task GetSvtNick(CommandContext ctx,
            [Description("Id, collectionNo or nickname of existing servant")] string servant)
        {
            try
            {
                Servant? svt;
                if (int.TryParse(servant, out int id))
                {
                    svt = Context.Servants.FirstOrDefault(x => x.Id == id || x.CollectionNo == id);
                }
                else svt = Context.ServantAliases.Include(x => x.Servant).FirstOrDefault(x => x.Nickname == servant)?.Servant;
                if (svt is null) throw new Exception("Servant could not be found");
                var builder = new DiscordEmbedBuilder();
                string str = string.Empty;
                foreach (var n in Context.ServantAliases.Where(x => x.Servant.Id == svt.Id))
                {
                    str += n.Nickname + "\n";
                }
                builder.AddField("Nicknames for servant " + (svt.NaName is null ? svt.JpName : svt.NaName), str);
                await ctx.Channel.SendMessageAsync(builder);
            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
            }
        }
        [Command("GetCeNames")]
        [Description("Get nicknames of a given craft essence")]
        public async Task GetCeNick(CommandContext ctx,
            [Description("Id, collectionNo or nickname of existing craft essence")] string craftessence)
        {
            try
            {
                CraftEssence? ce;
                if (int.TryParse(craftessence, out int id))
                {
                    ce = Context.CraftEssences.FirstOrDefault(x => x.Id == id || x.CollectionNo == id);
                }
                else ce = Context.CraftEssenceAliases.Include(x => x.CraftEssence).FirstOrDefault(x => x.Nickname == craftessence)?.CraftEssence;
                if (ce is null) throw new Exception("Servant could not be found");
                var builder = new DiscordEmbedBuilder();
                string str = string.Empty;
                foreach (var n in Context.CraftEssenceAliases.Where(x => x.CraftEssence.Id == ce.Id))
                {
                    str += n.Nickname + "\n";
                }
                builder.AddField("Nicknames for craft essence " + (ce.NaName is null ? ce.JpName : ce.NaName), str);
                await ctx.Channel.SendMessageAsync(builder);
            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
            }
        }
        [Command("GetQuestNames")]
        [Description("Get nicknames of a given quest")]
        public async Task GetQuestNick(CommandContext ctx,
            [Description("Id or nickname of existing quest")] string quest)
        {
            try
            {
                Quest? q;
                if (int.TryParse(quest, out int id))
                {
                    q = Context.Quests.FirstOrDefault(x => x.Id == id);
                }
                else q = Context.QuestAliases.Include(x => x.Quest).FirstOrDefault(x => x.Nickname == quest)?.Quest;
                if (q is null) throw new Exception("Quest could not be found");
                var builder = new DiscordEmbedBuilder();
                string str = string.Empty;
                foreach (var n in Context.QuestAliases.Where(x => x.Quest.Id == q.Id))
                {
                    str += n.Nickname + "\n";
                }
                builder.AddField("Nicknames for craft essence " + (q.NaName is null ? q.JpName : q.NaName), str);
                await ctx.Channel.SendMessageAsync(builder);
            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
            }
        }
        [Command("GetMcNames")]
        [Description("Get nicknames of a given quest")]
        public async Task GetMcNick(CommandContext ctx,
            [Description("Id or nickname of existing Mystic code")] string mysticCode)
        {
            try
            {
                MysticCode? mc;
                if (int.TryParse(mysticCode, out int id))
                {
                    mc = Context.MysticCodes.FirstOrDefault(x => x.Id == id);
                }
                else mc = Context.MysticCodeAliases.Include(x => x.MysticCode).FirstOrDefault(x => x.Nickname == mysticCode)?.MysticCode;
                if (mc is null) throw new Exception("Servant could not be found");
                var builder = new DiscordEmbedBuilder();
                string str = string.Empty;
                foreach (var n in Context.MysticCodeAliases.Where(x => x.MysticCode.Id == mc.Id))
                {
                    str += n.Nickname + "\n";
                }
                builder.AddField("Nicknames for craft essence " + (mc.NaName is null ? mc.JpName : mc.NaName), str);
                await ctx.Channel.SendMessageAsync(builder);
            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
            }
        }

        [Command("GetInfo")]
        [Description("Get all info of user")]
        public async Task GetRuns(CommandContext ctx,
            [Description("Id or name of user (Ex: _Dante09#9825)")] string name)
        {
            try
            {
                User? u;
                if (long.TryParse(name, out long id))
                {
                    u = Context.Users.FirstOrDefault(x => x.DiscordSnowflake == id);
                }
                else
                {
                    u = Context.Users.FirstOrDefault(x => x.DiscordUsername + "#" + x.DiscordDiscriminator == name);
                }
                if (u is null) throw new Exception($"{id} could not be found");


                var runs = Context.Runs.Where(x => x.Submitter == u).Include(r => r.Quest).Include(r => r.Party).ThenInclude(p => p.Servant).ToList();
                if (runs is not null && runs.Count > 0)
                {
                    string str = string.Empty;
                    DiscordEmbedBuilder builder = new DiscordEmbedBuilder();
                    foreach (var r in runs)
                    {
                        str += r.Quest.NaName is null ? r.Quest.JpName : r.Quest.NaName + "\n";
                        Servant? svt = r.Party?.FirstOrDefault(x => x.IsMainDps == true)?.Servant;
                        if (svt is not null) str += svt.NaName is null ? svt.JpName : svt.NaName;
                        str += "\n" + r.CreatedDate + ", Run id : " + r.Id;
                        str += "\n";
                    }
                    if (!string.IsNullOrEmpty(str)) builder.AddField("Submissions", str);
                    await ctx.Channel.SendMessageAsync(builder);
                }
                string s;
                var salias = Context.ServantAliases.Where(x => x.Submitter == u).Include(x => x.Servant).ToList();
                if (salias is not null && salias.Count > 0)
                {
                    var sBuilder = new DiscordEmbedBuilder();
                    s = string.Empty;
                    foreach (ServantAlias a in salias)
                    {
                        s += a.Nickname + " -> " + (a.Servant.NaName is null ? a.Servant.JpName : a.Servant.NaName);
                        s += "\n";
                    }
                    if (!string.IsNullOrEmpty(s)) sBuilder.AddField("Servant nicknames", s);
                    await ctx.Channel.SendMessageAsync(sBuilder);
                }


                var cealias = Context.CraftEssenceAliases.Where(x => x.Submitter == u).Include(x => x.CraftEssence).ToList();
                if (cealias is not null && cealias.Count > 0)
                {
                    var ceBuilder = new DiscordEmbedBuilder();
                    s = String.Empty;
                    foreach (CraftEssenceAlias a in cealias)
                    {
                        s += a.Nickname + " -> " + (a.CraftEssence.NaName is null ? a.CraftEssence.JpName : a.CraftEssence.NaName);
                        s += "\n";
                    }
                    if (!string.IsNullOrEmpty(s)) ceBuilder.AddField("Ce nicknames", s);
                }


                var qalias = Context.QuestAliases.Where(x => x.Submitter == u).Include(x => x.Quest).ToList();
                if (qalias is not null && qalias.Count > 0)
                {
                    var qBuilder = new DiscordEmbedBuilder();
                    s = String.Empty;
                    foreach (QuestAlias a in qalias)
                    {
                        s += a.Nickname + " -> " + (a.Quest.NaName is null ? a.Quest.JpName : a.Quest.NaName);
                        s += "\n";
                    }
                    if (!string.IsNullOrEmpty(s)) qBuilder.AddField("Quest nicknames", s);
                }

                var mcalias = Context.MysticCodeAliases.Where(x => x.Submitter == u).Include(x => x.MysticCode).ToList();
                if (mcalias is not null && mcalias.Count > 0)
                {
                    var mcBuilder = new DiscordEmbedBuilder();
                    s = String.Empty;
                    foreach (MysticCodeAlias a in mcalias)
                    {
                        s += a.Nickname + " -> " + (a.MysticCode.NaName is null ? a.MysticCode.JpName : a.MysticCode.NaName);
                        s += "\n";
                    }
                    if (!string.IsNullOrEmpty(s)) mcBuilder.AddField("Mc nicknames", s);
                }

            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
            }
        }
    }
}
