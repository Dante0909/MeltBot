// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using PassionLib.DAL;
using PassionLib.Models;


using (var context = new RunsContext())
// I think generally you'll want to pass around the same context to classes that want it
// rather than create & destroy them; probably do a singleton service like demo'd in:
// https://github.com/DSharpPlus/DSharpPlus/blob/master/docs/articles/commands/dependency_injection.md
// msg me if you need clarification
{
    var doDbRecreation = bool.Parse(Environment.GetEnvironmentVariable("DO_DB_RECREATION") ?? "false");
    if (doDbRecreation)
    {
        context.Database.EnsureDeleted();
        context.Database.Migrate();

        //await RunDbInitializer.Initialize(context);

        //var woahnilandRerunCq = context.Quests.FirstOrDefault(o => o.Id == 94042801);
        //context.Runs.Add(new Run(woahnilandRerunCq, "https://youtu.be/-BcOMkFBXng"));
        context.SaveChanges();
    }  
    
}
var bot = new MeltBot.Bot();
bot.RunAsync().GetAwaiter().GetResult();

