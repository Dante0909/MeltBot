using PassionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.DAL
{
    public static class RunDbInitializer
    {
        public static void Initialize(RunsContext context)
        {
            if (context.Runs.Any())
            {
                return; // DB already has data
            }

            var woahnilandRerunCq = context.Quests.FirstOrDefault(o => o.Id == 94042801);
            if(woahnilandRerunCq == null)
            {
                woahnilandRerunCq = new Quest(94042801,
                                              "【高難易度】護法少女スペシャルヒーローショー");
                woahnilandRerunCq.NaName =    "[High Difficulty] Magifender Girls Special Hero Show";
                context.Quests.Add(woahnilandRerunCq);
            }

            var runs = new Run[]
            {
                new Run(woahnilandRerunCq, "https://youtu.be/xqu9_kDYPvo"),
                new Run(woahnilandRerunCq, "https://youtu.be/JGo6nnMMu7g"),
                new Run(woahnilandRerunCq, "https://youtu.be/gwwTwJ1F9WY"),
                new Run(woahnilandRerunCq, "https://youtu.be/7ZrAoqPJ4qw")
           };
            context.Runs.AddRange(runs);

            context.SaveChanges();
        }
    }
}
