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
    //[ModuleLifespan(ModuleLifespan.Transient)]
    internal class DeleteCommands : BaseCommandModule
    {
        public RunsContext Context { private get; set; }
        public DiscordChannel DebugChannel { private get; set; }
        [Command("DeleteRun")]
        [Description("Deletes a run with a given id")]
        public async Task DeleteRun(CommandContext ctx,
            [Description("Id of the run to be deleted")] int runId)
        {
            try
            {

                Run? r = Context.Runs.FirstOrDefault(x => x.Id == runId);
                if (r is null) throw new Exception($"{r} could not be found");
                if (r.Submitter.DiscordSnowflake == (long)ctx.User.Id || Bot.Admin.ContainsKey(ctx.User.Id))
                {
                    Context.Runs.Remove(r);
                    Context.SaveChanges();
                }
                else throw new Exception("You do not have the required permissions to delete this run");
                await ctx.Channel.SendMessageAsync("Run successfully deleted");
            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
            }
        }

        [Command("DeleteNick")]
        public async Task DeleteNick(CommandContext ctx,
            [Description("Nickname to be removed")] string nickname)
        {
            try
            {
                Alias? a = Context.CraftEssenceAliases.FirstOrDefault(alias => alias.Nickname == nickname);
                if (a is null) a = Context.ServantAliases.FirstOrDefault(alias => alias.Nickname == nickname);
                if (a is null) a = Context.QuestAliases.FirstOrDefault(alias => alias.Nickname == nickname);
                if (a is null) a = Context.MysticCodeAliases.FirstOrDefault(alias => alias.Nickname == nickname);
                if (a is null) throw new Exception($"{nickname} could not be found");
                Type t
                if (a.Submitter.DiscordSnowflake == (long)ctx.User.Id || Bot.Admin.ContainsKey(ctx.User.Id))
                {
                    t = a.GetType();
                    if (t == typeof(CraftEssenceAlias)) Context.CraftEssenceAliases.Remove((CraftEssenceAlias)a);
                    if (t == typeof(QuestAlias)) Context.QuestAliases.Remove((QuestAlias)a);
                    if (t == typeof(ServantAlias)) Context.ServantAliases.Remove((ServantAlias)a);
                    if (t == typeof(MysticCodeAlias)) Context.MysticCodeAliases.Remove((MysticCodeAlias)a);
                }
                else throw new Exception("You do not have permissions to delete this nickname");
                Context.SaveChanges();
                await ctx.Channel.SendMessageAsync($"Nickname {nickname} was removed for type " + t.ToString());
            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
            }

        }

    }
}
