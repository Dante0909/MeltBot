using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using PassionLib.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeltBot
{
    internal class Bot
    {
        public static readonly Dictionary<ulong, string> Admin = new Dictionary<ulong, string>()
        {
            {290938252540641290,"_Dante09" }
        };
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public async Task RunAsync()
        {
            var config = new DiscordConfiguration
            {

                //Token = Environment.GetEnvironmentVariable("TOKEN"),
                //Couldn't get the environment variable 
                Token = "ODc4MTM3MjQ3NzA1MjEwODkw.YR8zCg.41ZpvG40rApxzQrt5C-4FwKLsf8",
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,

            };
            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            DiscordChannel d = await Client.GetChannelAsync(918986606864629810);
            var services = new ServiceCollection().AddSingleton<RunsContext>().AddSingleton<DiscordChannel>(d).BuildServiceProvider();

            var commandsConfig = new CommandsNextConfiguration
            {
                Services = services,
                IgnoreExtraArguments = true,
                StringPrefixes = new string[] { "%" },
                EnableDms = true,
                EnableMentionPrefix = true
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<Modules.Commands>();
            Commands.RegisterCommands<Modules.AddCommands>();
            Commands.RegisterCommands<Modules.DeleteCommands>();

            var activity = new DSharpPlus.Entities.DiscordActivity("%help", DSharpPlus.Entities.ActivityType.ListeningTo);


            await Client.ConnectAsync(activity);

            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            Console.WriteLine("Logged In");

            return Task.CompletedTask;
        }
    }
}

