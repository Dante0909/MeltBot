using DSharpPlus;
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
    //[ModuleLifespan(ModuleLifespan.Transient)]
    internal class Commands : BaseCommandModule
    {
        public RunsContext Context { private get; set; }
        public DiscordChannel DebugChannel { private get; set; }

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
                    await ctx.Channel.SendMessageAsync(ctx.User.Mention);
                    await ctx.Channel.SendMessageAsync(mention);

                    Context.Pongs.Add(new Pong(mention));
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message + "\n" + ex.StackTrace);
                await ctx.Channel.SendMessageAsync(ex.ToString());
            }
        }

        [Hidden]
        [Command("woahreceive")]
        public async Task AddPing(CommandContext ctx)
        {
            try
            {
                if (Context.Pongs.Any(x => x.UserMention == ctx.User.Mention)) throw new Exception("You are already added <:woah:802188856686411783>");
                Context.Pongs.Add(new Pong(ctx.User.Mention));

                Context.SaveChanges();
                await ctx.Channel.SendMessageAsync("Added " + ctx.User.Mention + " <:woah:802188856686411783>");
            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
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
                await Commands.SendDebug(ctx, ex, DebugChannel);
            }
        }

        [Hidden]
        [Command("woahditch")]
        public async Task ByePing(CommandContext ctx)
        {
            if (ctx.Channel.Id == 875075360587403304)
            {
                try
                {
                    Pong? p = Context.Pongs.Where(x => x.UserMention == ctx.User.Mention).FirstOrDefault();
                    if (p is not null)
                    {
                        Context.Pongs.Remove(p);

                        Context.SaveChanges();
                        await ctx.Channel.SendMessageAsync("Sad to see you leave :woahpium:").ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    await Commands.SendDebug(ctx, ex, DebugChannel);
                }
            }

        }
        public static async Task Init(DiscordClient sender)
        {
            await sender.SendMessageAsync(await sender.GetChannelAsync(878138355945185334), "ack");
            int counter = 0;
            while (sender is not null)
            {

                DateTime pingtime = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc).AddMinutes(1423);

                if (DateTime.UtcNow > pingtime) pingtime = pingtime.AddDays(1);
                await Task.Delay((int)pingtime.Subtract(DateTime.UtcNow).TotalMilliseconds);
                var thread = await sender.GetChannelAsync(875075360587403304).ConfigureAwait(false);
                using (var r = new RunsContext())
                {
                    if (thread is not null)
                    {

                        string message = "<a:woahgiver:911084288705986570>";
                        foreach (Pong p in r.Pongs)
                        {
                            message += " " + p.UserMention;
                        }
                        message += " \n use %woahreceive to get blessed by melt";
                        await thread.SendMessageAsync(message).ConfigureAwait(false);
                    }
                    if (counter == 0)
                    {
                        var gameplay = await sender.GetChannelAsync(715944125916250154).ConfigureAwait(false);
                        foreach (var t in gameplay.Threads)
                        {
                            var d = await t.SendMessageAsync("Weekly message to keep this thread alive").ConfigureAwait(false);
                            await Task.Delay(1000);
                            await d.DeleteAsync();
                        }
                        counter++;
                    }
                    else if (counter == 6) counter = 0;

                }
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
                    await ctx.Channel.SendMessageAsync("accepted");

                    int counter = 0;

                    var thread = await ctx.Client.GetChannelAsync(875075360587403304).ConfigureAwait(false);

                    while (true)
                    {
                        string message = "<a:woahgiver:911084288705986570>";
                        foreach (Pong p in Context.Pongs)
                        {
                            message += " " + p.UserMention;
                        }
                        await thread.SendMessageAsync(message);
                        await Task.Delay(995 * 60 * 60 * 24).ConfigureAwait(false);
                        if (counter == 3)
                        {
                            var gameplay = await ctx.Client.GetChannelAsync(715944125916250154).ConfigureAwait(false);
                            foreach (var t in gameplay.Threads)
                            {
                                var d = await t.SendMessageAsync("Weekly message to keep this thread alive");
                                await Task.Delay(1000);
                                await d.DeleteAsync();
                            }
                            counter = 0;
                        }
                        else counter++;


                        //await thread.SendMessageAsync(":woahgiver: " + "<@!91383118644154368> <@!383990559070486529> <@!231155913430401035> <@!273449958152077312> <@!357729894765035520> <@!285701533583015936>").ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static async Task SendDebug(CommandContext ctx, Exception ex, DiscordChannel d)
        {
            Console.WriteLine(ex);
            await ctx.Channel.SendMessageAsync(ex.Message + "\nMessage _Dante09#9825 if more help is needed");
            await d.SendMessageAsync("Time : " + DateTime.UtcNow + "\nUser : " + ctx.User.Username + "#" + ctx.User.Discriminator + " " + ctx.User.Id + "\nGuild : " + ctx?.Guild?.Name + "\n" + ex.Message + "\n" + ex.StackTrace);
            await d.SendMessageAsync(ex?.InnerException?.ToString());
        }
        [Command("website")]
        public async Task Site(CommandContext ctx)
        {
            try
            {
                await ctx.Channel.SendMessageAsync("https://combatrecords.xxil.cc/jp");
            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
            }
        }
        [Command("run")]
        [Description("Submit run into database")]
        public async Task Run(CommandContext ctx,
            [Description("Name or id of the quest")] string quest,
            [Description("Link of the run")] string runUrl,
            [Description("Additional params")] params string[] args)
        {
            try
            {
                var user = DbHelper.GetUser(ctx, Context);
                List<PartySlot>? party = null;//Insert your program output
                Quest q = DbHelper.GetQuest(Context, quest);
                Run run = DbHelper.CreateRun(Context, q, runUrl, user, party, null, args);

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
                await ctx.Channel.SendMessageAsync("Run successfully added, run id is " + run.Id);

            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
            }

        }
        [Command("Edit")]
        [Description("Edit existing submission")]
        public async Task Edit(CommandContext ctx,
            [Description("Id of the run")] int id,
            [Description("Same params as %run")] params string[] args)
        {
            try
            {
                var r = Context.Runs.Include(x => x.Party).Include(x => x.Quest).Include(x => x.MysticCode).FirstOrDefault(x => x.Id == id);
                if (r is null) throw new Exception("Run id could not be found");
                if (Bot.Admin.ContainsKey(ctx.User.Id) || ctx.User.Id == (ulong)r.Submitter.DiscordSnowflake)
                {
                    r = DbHelper.CreateRun(Context, r.Quest, r.RunUrl, r.Submitter, null, r, args);
                    if (args.Contains("debug"))
                    {
                        string obj = JsonConvert.SerializeObject(r, Formatting.Indented);
                        byte[] t = Encoding.UTF8.GetBytes(obj);
                        using (var stream = new MemoryStream(t))
                        {
                            var builder = new DiscordMessageBuilder();
                            builder.WithFile("run.txt", stream);
                            await ctx.Channel.SendMessageAsync(builder).ConfigureAwait(false);
                        }
                    }

                    Context.SaveChanges();
                    await ctx.Channel.SendMessageAsync("Run edited");

                }
                else throw new Exception("You do not have permission to edit this run");
            }
            catch (Exception ex)
            {
                await Commands.SendDebug(ctx, ex, DebugChannel);
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

