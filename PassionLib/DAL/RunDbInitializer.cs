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
                Console.WriteLine("herre");
                return; // DB already has data
            }
            var woahnilandRerunCq = context.Quests.FirstOrDefault(o => o.Id == 94042801);
            if (woahnilandRerunCq == null)
            {
                woahnilandRerunCq = new Quest(94042801,
                                              "【高難易度】護法少女スペシャルヒーローショー");
                woahnilandRerunCq.NaName = "[High Difficulty] Magifender Girls Special Hero Show";
                context.Quests.Add(woahnilandRerunCq);
            }
            var imaginaryScramble = context.Quests.FirstOrDefault(q => q.Id == 94053435);
            if(imaginaryScramble is null)
            {
                imaginaryScramble = new Quest(94053435, "【高難易度】聖女を呼ぶ声");
                context.Quests.Add(imaginaryScramble);
            }
            var charlotte = context.Servants.FirstOrDefault(x => x.Id == 603800);

            if (charlotte is null)//oof
            {
                charlotte = new Servant(603800, "シャルロット・コルデー", 1);
                //using(var client = new HttpClient())
                //{
                //    var response = client.GetStringAsync("https://api.atlasacademy.io/basic/JP/Servant/603800").ConfigureAwait(false);
                //}
                //charlotte = new Servant()
                context.Servants.Add(charlotte);
            }
            User u = new User();
            var runs = new Run[]
            {
                new Run(woahnilandRerunCq,"https://youtu.be/xqu9_kDYPvo",charlotte, u)
                {
                    Party = new List<PartySlot>()
                    {
                        new PartySlot()
                        {


                        }
                    }
                }
            //new Run(woahnilandRerunCq, "https://youtu.be/JGo6nnMMu7g"),
            //new Run(woahnilandRerunCq, "https://youtu.be/gwwTwJ1F9WY"),
            //new Run(woahnilandRerunCq, "https://youtu.be/7ZrAoqPJ4qw")
            };
            context.Runs.AddRange(runs);

            context.SaveChanges();
        }
    }
}
