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
        [Command("start")]
        public async Task Start(CommandContext ctx)
        {
            if (ctx.User.Id == 290938252540641290)
            {

            }
        }

        [Command("temprun")]
        public async Task TempRun(CommandContext ctx,
            [Description("Name or id of the quest")] string quest,
            [Description("Name or id of the dps")] string dps,
            [Description("Link of the run")] string runLink,
            [Description("Additional params")] params string[] link)
        {

            try
            {
                User user = null;

                var q = GetQuest(quest);
                var d = GetServant(dps);


                var discordUser = ctx.User;
                var users = Context.Users.Where(u => u.DiscordSnowflake == (long)discordUser.Id).Select(u => u).ToList();
                if (users is not null && users.Any())
                {
                    foreach (var u in users)
                    {
                        if (discordUser.Discriminator == u.DiscordDiscriminator && discordUser.Username == u.DiscordUsername)
                        {
                            user = u;
                            //means that the exact user was found
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
                //var run = new Run()
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await ctx.Channel.SendMessageAsync(ex.Message);
            }

        }

        //These two commmands should not be in this class
        private Quest GetQuest(string quest)
        {
            Quest q = null;
            if (int.TryParse(quest, out int id))
            {
                q = Context.Quests.Where(x => x.Id == id).FirstOrDefault();
                if (q is null) throw new Exception($"Quest {quest} could not be found.");
            }
            else
            {
                q = Context.QuestAliases.Where(x => x.Nickname == quest).FirstOrDefault()?.Quest;
                if (q is null) throw new Exception($"Quest {quest} could not be found.");

            }
            return q;
        }

        private Servant GetServant(string dps)
        {
            Servant d = null;
            if (int.TryParse(dps, out int id))
            {
                d = Context.Servants.Where(x => x.Id == id).FirstOrDefault();
                if (d is null) throw new Exception($"Servant {dps} could not be found.");
            }
            else
            {
                d = Context.ServantAliases.Where(x => x.Nickname == dps).FirstOrDefault()?.Servant;
                if (d is null) throw new Exception($"Servant {dps} could not be found.");
            }
            return d;
        }
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

