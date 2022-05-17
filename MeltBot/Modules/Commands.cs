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
        public Random Random { private get; set; }


        [Hidden]
        [Command("ping")]
        [Description("Returns pong")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
            Random random = new Random(ctx.Message.Timestamp.Millisecond);
            Console.WriteLine(random.Next(0,12));
        }

        [Hidden]
        [Command("cereal")]
        public async Task Cereal(CommandContext ctx)
        {
            if (!Context.Cereal.Any())
            {
                Context.Add(new CerealShrine());
                Context.SaveChanges();
            }

            Pongv2? p = Context.Pongv2.FirstOrDefault(x => x.Id == ctx.Member.Id);
            if(p is null)
            {
                p = new Pongv2(ctx.Member.Id, false);
                Context.Pongv2.Add(p);
                Context.SaveChanges();
            }
            if (ctx.Channel.Id == 875075360587403304)
            {
                if (Random.Next(0, 100) == 0)
                {
                    await ctx.Channel.SendMessageAsync("Trully blessed, two prayers have been sent to his shrine");
                    if (Context.Cereal.First().SendPrayer(p))
                    {
                        var c = await ctx.Client.GetChannelAsync(875075360587403304);
                        await c.SendMessageAsync("<@141381999674785792> :dalaobow:");
                    }
                }
                else
                {
                    await ctx.Channel.SendMessageAsync("A prayer has been sent to his shrine");
                }          
            }
            else
            {
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pray:", false));
            }
            if (Context.Cereal.First().SendPrayer(p))
            {
                    var c = await ctx.Client.GetChannelAsync(875075360587403304);
                    await c.SendMessageAsync("<@141381999674785792> <a:DalaoBow:875090629292613742>");         
            }
            Context.SaveChanges();
        }
        [Hidden]
        [Command("cerealmostping")]
        public async Task CerealMostPing(CommandContext ctx, bool log = false)
        {
            if(ctx.User.Id == 290938252540641290)
            {
                List<Pongv2> most = new List<Pongv2>();
                foreach (var v in Context.Pongv2)
                {
                    if(log) await ctx.Channel.SendMessageAsync(v.UserMention() + " " + v.LastSummonCount).ConfigureAwait(false);
                    if (v.LastSummonCount == (most.Max(x => x.LastSummonCount) ?? 0)) most.Add(v);
                    else if (v.LastSummonCount > (most.Max(x => x.LastSummonCount) ?? 0))
                    {
                        most.Clear();
                        most.Add(v);
                    }

                }
                StringBuilder sb = new StringBuilder();
                foreach (var v in most)
                {
                    sb.Append(v.UserMention() + " ");

                }
                sb.Append("\nYou have sent the most last prayer. A gift awaits you..");

                
                await ctx.Channel.SendMessageAsync(sb.ToString()).ConfigureAwait(false);
            }
            
            
        }

        [Hidden]
        [Command("woahreceive")]
        public async Task AddPing(CommandContext ctx, ulong mention)
        {
            try
            {
                if (ctx.User.Id == 290938252540641290)
                {
                    var p = Context.Pongv2.FirstOrDefault(x => x.Id == mention);

                    if (p is not null) p.ToBePinged = true;
                    else Context.Pongv2.Add(new Pongv2(mention, true));
                    await ctx.Channel.SendMessageAsync(mention + "added");

                    
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
        [Command("wtfping")]
        public async Task WtfPing(CommandContext ctx, ulong id)
        {
            if(ctx.User.Id == 290938252540641290)
            {
                var p = Context.Pongv2.First(x => x.Id == id);
                Context.Pongv2.Remove(p);
                await Context.SaveChangesAsync();
            }            
        }
        [Hidden]
        [Command("woahreceive")]
        public async Task AddPing(CommandContext ctx)
        {
            try
            {
                var a = Context.Pongv2.First(x=>x.Id == ctx.User.Id);
                if (a is null) Context.Pongv2.Add(new Pongv2(ctx.User.Id, true));
                else
                {
                    if (a.ToBePinged == true) throw new CustomException("You are already added <:woah:802188856686411783>");
                    else a.ToBePinged = true;
                }
                
                

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
        public async Task ByePing(CommandContext ctx, ulong mention)
        {
            try
            {
                if (ctx.User.Id == 290938252540641290)
                {
                    Pongv2? p = Context.Pongv2.FirstOrDefault(x => x.Id == mention);
                    if (p is not null)
                    {
                        p.ToBePinged = false;
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
                    Pongv2? p = Context.Pongv2.FirstOrDefault(x => x.Id == ctx.User.Id);
                    if (p is not null)
                    {
                        p.ToBePinged = false;

                        Context.SaveChanges();
                        await ctx.Channel.SendMessageAsync("Sad to see you leave " + ctx.User.Mention + " :woahpium:").ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    await Commands.SendDebug(ctx, ex, DebugChannel);
                }
            }
           
        }
        [Hidden]
        [Command("psa")]
        public async Task Psa(CommandContext ctx, string message)
        {
            if(ctx.User.Id == 290938252540641290)
            {
                var thread = await ctx.Client.GetChannelAsync(875075360587403304).ConfigureAwait(false);
                if(thread is not null)
                {
                    await thread.SendMessageAsync(message).ConfigureAwait(false);
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
                        if (sender?.CurrentUser?.Presence?.Status == UserStatus.Online)
                        {
                            var shrine = r.Cereal.Include(x => x.LastPong).First();
                            Pongv2? last = shrine.LastPong;
                            if(last is not null) last.LastSummonCount++;

                            string message = "<a:woahgiver:911084288705986570>";
                            foreach (Pongv2 p in r.Pongv2.Where(x=>x.ToBePinged == true))
                            {
                                message += $" {p.UserMention()}";
                            }
                            message += " \n use %woahreceive to get blessed by melt";
                            await thread.SendMessageAsync(message).ConfigureAwait(false);
                            await Task.Delay(60000);
                            await thread.SendMessageAsync($"The last person to send a prayer is {last?.UserMention()}");
                            var c = r.Cereal.First();
                            if (c.LowerCountdown() <= 0)
                            {
                                List<Pongv2> most = new List<Pongv2>();
                                foreach (var v in r.Pongv2)
                                {
                                    if (v.LastSummonCount == (most.Max(x => x.LastSummonCount) ?? 0)) most.Add(v);
                                    else if (v.LastSummonCount > (most.Max(x => x.LastSummonCount) ?? 0))
                                    {
                                        most.Clear();
                                        most.Add(v);
                                    }

                                }

                                StringBuilder sb = new StringBuilder();
                                foreach (var v in most)
                                {
                                    sb.Append(v.UserMention() + " ");

                                }
                                sb.Append("\nYou have sent the most last prayer. A gift awaits you..");


                                await thread.SendMessageAsync(sb.ToString()).ConfigureAwait(false);

                                foreach (var v in r.Pongv2)
                                {
                                    v.LastSummonCount = 0;
                                }
                                c.Countdown = 29;
                            }
                            else
                            {
                                await thread.SendMessageAsync($"{c.Countdown} more day(s) remaining");
                            }
                            r.SaveChanges();

                        }
                        else
                        {
                            break;
                        }
                        
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
        
        public static async Task SendDebug(CommandContext ctx, Exception ex, DiscordChannel d)
        {
            Console.WriteLine(ex);
            await ctx.Channel.SendMessageAsync(ex.Message + "\nMessage _Dante09#9825 if more help is needed");
            if(ex is not CustomException)
            {
                await d.SendMessageAsync("Time : " + DateTime.UtcNow + "\nUser : " + ctx.User.Username + "#" + ctx.User.Discriminator + " " + ctx.User.Id + "\nGuild : " + ctx?.Guild?.Name + "\n" + ex.Message + "\n" + ex.StackTrace);
                await d.SendMessageAsync(ex?.InnerException?.ToString());
            }
            
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
                if (r is null) throw new CustomException("Run id could not be found");
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
                else throw new CustomException("You do not have permission to edit this run");
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

