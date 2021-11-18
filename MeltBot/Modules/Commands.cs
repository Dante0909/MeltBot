using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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
                    var thread = await ctx.Client.GetChannelAsync(875075360587403304).ConfigureAwait(false);
                    while (true)
                    {
                        string message = ":woahgiver: ";
                        foreach (Pong p in Context.Pongs)
                        {
                            message += p.UserMention + " ";
                        }
                        await ctx.Channel.SendMessageAsync(message);
                        //await thread.SendMessageAsync(":woahgiver: " + "<@!91383118644154368> <@!383990559070486529> <@!231155913430401035> <@!273449958152077312> <@!357729894765035520> <@!285701533583015936>").ConfigureAwait(false);
                        await Task.Delay(1000 * 60 * 60 * 24).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        [Command("temprun")]
        public async Task TempRun(CommandContext ctx,
            [Description("Name or id of the quest")] string quest,
            [Description("Name or id of the dps")] string dps,
            [Description("Link of the run")] string runUrl,
            [Description("Additional params")] params string[] args)
        {
            try
            {
                User user = null;
                var discordUser = ctx.User;
                var users = Context.Users.Where(u => u.DiscordSnowflake == (long)discordUser.Id)?.ToList();
                if (users is not null && users.Any())
                {
                    foreach (var u in users)
                    {
                        if (discordUser.Discriminator == u.DiscordDiscriminator && discordUser.Username == u.DiscordUsername)
                        {
                            //means that the exact user was found
                            user = u;
                            break;
                        }
                    }
                }
                if (user is null) user = new User()
                {
                    DiscordDiscriminator = discordUser.Discriminator,
                    DiscordSnowflake = (long)discordUser.Id,
                    DiscordUsername = discordUser.Username
                };
                List<PartySlot>? party = null;//Insert your program output
                Run run = DbHelper.CreateRun(Context, quest, runUrl, dps, user, party, args);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await ctx.Channel.SendMessageAsync(ex.Message);
            }

        }

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

