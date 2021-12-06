using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PassionLib.DAL;
using PassionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeltBot.Modules
{
    internal class Commands : BaseCommandModule
    {
        public RunsContext Context { private get; set; }

        [Command("ping")]
        [Description("Returns pong")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }
        [Hidden]
        [Command("woahreceive")]
        public async Task AddPing(CommandContext ctx, string mention)
        {
            try
            {
                if (ctx.User.Id == 290938252540641290)
                {
                    Context.Pongs.Add(new Pong() { UserMention = mention });
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await ctx.Channel.SendMessageAsync(ex.Message);
            }
        }

        [Hidden]
        [Command("woahreceive")]
        public async Task AddPing(CommandContext ctx)
        {
            try
            {
                Context.Pongs.Add(new Pong() { UserMention = ctx.User.Mention });
                await ctx.Channel.SendMessageAsync("Embrace woah");
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await ctx.Channel.SendMessageAsync(ex.Message);
            }
        }
        [Hidden]
        [Command("woahditch")]
        public async Task ByePing(CommandContext ctx, string mention)
        {
            try
            {
                if (ctx.User.Id == 290938252540641290)
                {
                    Pong? p = Context.Pongs.Where(x => x.UserMention == mention).FirstOrDefault();
                    if (p is not null)
                    {
                        Context.Pongs.Remove(p);
                        Context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await ctx.Channel.SendMessageAsync(ex.Message);
            }
        }

        [Hidden]
        [Command("woahditch")]
        public async Task ByePing(CommandContext ctx)
        {
            try
            {
                Pong? p = Context.Pongs.Where(x => x.UserMention == ctx.User.Mention).FirstOrDefault();
                if (p is not null)
                {
                    Context.Pongs.Remove(p);
                    await ctx.Channel.SendMessageAsync("Sad to see you leave :woahpium:");
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await ctx.Channel.SendMessageAsync(ex.Message);
            }
        }

        [Hidden]
        [Command("start")]
        public async Task Start(CommandContext ctx)
        {
            try
            {
                if (ctx.User.Id == 290938252540641290)
                {
                    int counter = 0;

                    var thread = await ctx.Client.GetChannelAsync(875075360587403304).ConfigureAwait(false);
                    while (true)
                    {
                        await Task.Delay(995 * 60 * 60 * 24).ConfigureAwait(false);
                        if (counter == 7)
                        {
                            var gameplay = await ctx.Client.GetChannelAsync(715944125916250154).ConfigureAwait(false);
                            foreach (var t in gameplay.Threads)
                            {
                                await t.SendMessageAsync("Weekly message to keep this thread alive");
                            }
                            counter = 0;
                        }
                        else counter++;

                        string message = "<a:woahgiver:911084288705986570>";
                        foreach (Pong p in Context.Pongs)
                        {
                            message += " " + p.UserMention;
                        }
                        //await thread.SendMessageAsync(message);
                        //await thread.SendMessageAsync(":woahgiver: " + "<@!91383118644154368> <@!383990559070486529> <@!231155913430401035> <@!273449958152077312> <@!357729894765035520> <@!285701533583015936>").ConfigureAwait(false);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        [Command("GetInfo")]
        [Description("Get all runs submitted by a given user")]
        public async Task GetRuns(CommandContext ctx,
            [Description("Id or discord name+tag (ex: _Dante09#9825)")] string name)
        {
            try
            {
                if (Bot.Admin.ContainsKey(ctx.User.Id))
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
                    DiscordEmbedBuilder builder = new DiscordEmbedBuilder();
                    string str = string.Empty;
                    foreach (var r in Context.Runs.Where(x => x.Submitter == u))
                    {
                        str += r.Id + " ";
                        str += r.Quest.NaName is null ? r.Quest.JpName : r.Quest.NaName + " ";
                        Servant? svt = r.Party?.FirstOrDefault(x => x.IsMainDps == true)?.Servant;
                        if (svt is not null) str += svt.NaName is null ? svt.JpName : svt.NaName;
                        str += r.CreatedDate;
                        str += "\n";
                    }

                    if (!string.IsNullOrEmpty(str)) builder.AddField("runs", str);
                    var l = new List<Alias>();
                    l.AddRange(Context.QuestAliases.Where(x => x.Submitter == u));
                    l.AddRange(Context.ServantAliases.Where(x => x.Submitter == u));
                    l.AddRange(Context.CraftEssenceAliases.Where(x => x.Submitter == u));
                    l.AddRange(Context.MysticCodeAliases.Where(x => x.Submitter == u));
                    string s = string.Empty;
                    foreach (ServantAlias a in Context.ServantAliases.Where(x => x.Submitter == u))
                    {
                        s += a.Nickname + " -> " + (a.Servant.NaName is null ? a.Servant.JpName : a.Servant.NaName);
                        s += "\n";
                    }
                    if (!string.IsNullOrEmpty(s)) builder.AddField("Servant nicknames", s);
                    s = String.Empty;
                    foreach (CraftEssenceAlias a in Context.CraftEssenceAliases.Where(x => x.Submitter == u))
                    {
                        s += a.Nickname + " -> " + (a.CraftEssence.NaName is null ? a.CraftEssence.JpName : a.CraftEssence.NaName);
                        s += "\n";
                    }
                    if (!string.IsNullOrEmpty(s)) builder.AddField("Ce nicknames", s);
                    s = String.Empty;
                    foreach (QuestAlias a in Context.QuestAliases.Where(x => x.Submitter == u))
                    {
                        s += a.Nickname + " -> " + (a.Quest.NaName is null ? a.Quest.JpName : a.Quest.NaName);
                        s += "\n";
                    }
                    if (!string.IsNullOrEmpty(s)) builder.AddField("Quest nicknames", s);
                    s = String.Empty;
                    foreach (MysticCodeAlias a in Context.MysticCodeAliases.Where(x => x.Submitter == u))
                    {
                        s += a.Nickname + " -> " + (a.MysticCode.NaName is null ? a.MysticCode.JpName : a.MysticCode.NaName);
                        s += "\n";
                    }
                    if (!string.IsNullOrEmpty(s)) builder.AddField("Mc nicknames", s);
                    s = String.Empty;

                    await ctx.Channel.SendMessageAsync(builder);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await ctx.Channel.SendMessageAsync(ex.Message);
            }
        }
        [Command("website")]
        public async Task Site(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("https://combatrecords.xxil.cc/jp");
        }
        [Command("temprun")]
        public async Task TempRun(CommandContext ctx,
            [Description("Name or id of the quest")] string quest,
            [Description("Link of the run")] string runUrl,
            [Description("Additional params")] params string[] args)
        {
            try
            {
                var user = DbHelper.GetUser(ctx, Context);
                List<PartySlot>? party = null;//Insert your program output
                Run run = DbHelper.CreateRun(Context, quest, runUrl, user, party, args);
                //Context.Runs.Add(run);
                //Context.SaveChanges();
                if (args.Contains("debug"))
                {
                    string obj = JsonConvert.SerializeObject(run, Formatting.Indented);
                    byte[] t = Encoding.UTF8.GetBytes(obj);
                    using (var stream = new MemoryStream(t))
                    {
                        var builder = new DiscordMessageBuilder();
                        builder.WithFile("run.txt", stream);
                        await ctx.Channel.SendMessageAsync(builder).ConfigureAwait(false);
                    }
                }

                Context.Runs.Add(run);
                Context.SaveChanges();
                await ctx.Channel.SendMessageAsync("Run successfully added");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await ctx.Channel.SendMessageAsync(ex.Message);
            }

        }
        //[Hidden]
        //[Command("deletedb")]
        //public async Task AdminDelete(CommandContext ctx)
        //{
        //    try
        //    {
        //        if (ctx.User.Id == 290938252540641290 || ctx.User.Id == 91383118644154368)
        //        {
        //            Context.Database.ExecuteSqlRaw("delete from \"Quests\" cascade");
        //            Context.Database.ExecuteSqlRaw("delete from \"Runs\" cascade");
        //            Context.Database.ExecuteSqlRaw("delete from \"Users\" cascade");
        //            Context.Database.ExecuteSqlRaw("delete from \"CraftEssences\" cascade");
        //            Context.Database.ExecuteSqlRaw("delete from \"MysticCodes\" cascade");
        //            Context.Database.ExecuteSqlRaw("delete from \"Servants\" cascade");
        //            Context.Database.ExecuteSqlRaw("delete from \"CraftEssenceAliases\" cascade");
        //            Context.Database.ExecuteSqlRaw("delete from \"MysticCodeAliases\" cascade");
        //            Context.Database.ExecuteSqlRaw("delete from \"QuestAliases\" cascade");
        //            Context.Database.ExecuteSqlRaw("delete from \"ServantAliases\" cascade");
        //            Context.SaveChanges();

        //            await ctx.Channel.SendMessageAsync("Db deleted and created");

        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        await ctx.Channel.SendMessageAsync(ex.ToString());
        //    }

        //}
        //These two commmands should not be in this class

        //I commented all the commands, I just copypastad commands that are old and outdated.
        /*
        [Command("run")]
        [Description("Add a run to a sheet. Use quotations to input arguments with spaces. Ex: \"dual core\"")]
        public async Task Run(CommandContext ctx,
            [Description("Name of the event")] string eventName,
            [Description("Main damage dealer")] string dps,
            [Description("Link of the run")] string link,
            [Description("Description")] string description = "")

        {
            if (Program.ContainsSheet(eventName))
            {
                //Console.WriteLine("Entry succesful");
                Program.CreateEntry(eventName, dps, link, description);
                await ctx.Channel.SendMessageAsync($"Successfully added in { eventName }").ConfigureAwait(false);
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"Could not add in { eventName }, is the spelling right?").ConfigureAwait(false);
            }


            //int sheetId;
            //if(int.TryParse(eventName, out int result))
            //{
            //    sheetId = result;   
            //}
            //else sheetId = Program.GetSheetIdFromArgument(eventName);

            //if (sheetId == -1)
            //{
            //    await ctx.Channel.SendMessageAsync("Could not find matching event.");
            //}
            //else
            //{
            //    Program.CreateEntry(eventName, dps, link, description);
            //    await ctx.Channel.SendMessageAsync($"Successfully added in { eventName }").ConfigureAwait(false);
            //}

        }
        [Command("add")]
        [Description("Add a sheet to the spreadsheet.")]
        public async Task Add(CommandContext ctx,
            [Description("The Id of the quest")] string questId,
            [Description("The name of the sheet")] string sheetName)
        {
            if (int.TryParse(questId, out int _) && Program.CreateSheet(questId, sheetName))
            {
                var test = ctx.Channel.GetMessagesAsync().Result;

                string attachmentlink = test[0].Attachments[0].Url;

                await ctx.Channel.SendMessageAsync($"Succesfully added sheet {sheetName} with id {questId}.\n" +
                    $"<https://apps.atlasacademy.io/db/JP/quest/{questId}/1>").ConfigureAwait(false);
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Make sure the quest id is a number. This sheet name or quest id might exist already.").ConfigureAwait(false);
            }
            
        }
        [Command("download")]
        [Description("Download sheet and submissions")]
        public async Task Download(CommandContext ctx)
        {
            if (Program.Admin.ContainsKey(ctx.User.Id.ToString()))
            {
                using (var stream = new FileStream(Program.Directory + "/sheets.json", FileMode.Open, FileAccess.Read))
                {
                    var builder = new DiscordMessageBuilder();
                    builder.WithFile(stream);
                    await ctx.Channel.SendMessageAsync(builder).ConfigureAwait(false);
                }
            }

        }
        [Command("sheet")]
        [Description("Get the sheet link")]
        public async Task Sheet(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync(Program.spreadsheetLink).ConfigureAwait(false);
        }
        //[Command("edit")]
        //[Description("Edit Sheet Name")]
        //public async Task Edit(CommandContext ctx, int sheetId, string newSheetName)
        //{
        //    if (Program.Admin.ContainsKey(ctx.User.Id.ToString()))
        //    {

        //    }
        //}
        */
    }

}

