using PassionLib.DAL;
using PassionLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeltBot
{
    internal static class DbHelper
    {
        public static Run CreateRun(RunsContext context, string strQuest, string runUrl, string strDps, User user, List<PartySlot>? party, params string[] args)
        {
            Quest quest = GetQuest(context, strQuest);
            Servant dps = GetServant(context, strDps);
            if (Uri.IsWellFormedUriString(runUrl, UriKind.Absolute)) throw new Exception($"Invalid Url format : {runUrl}");
            if (context.Runs.Where(x => x.RunUrl == runUrl).Any()) throw new Exception("A run with the provided link already exists.");
            Run run = new Run(quest, runUrl, dps, user);
            if (party is not null)
            {

                run.Party = party;
                //ask if craft essence/servant are null
            }
            /*
            if (args is not null && !args.Any())
            {
                for (int i = 0; i < args.Length; ++i)
                {
                    Servant servant = null;
                    string s = args[i].ToLower();
                    if (s == "f")
                    {
                        run.Failure = true;
                        continue;
                    }
                    if (s.StartsWith("cs"))
                    {
                        s = s.Substring(2);
                        if(short.TryParse(s, out short n))
                        {
                            run.CsUsed = n;
                        }
                        else
                        {
                            run.CsUsed = 1;
                        }
                        continue;
                    }
                    if (s.StartsWith("r"))
                    {
                        s = s.Substring(1);
                        if (short.TryParse(s, out short n))
                        {
                            run.RevivesUsed = n;
                        }
                        else
                        {
                            run.RevivesUsed = 1;
                        }
                        continue;
                    }
                    if (s.StartsWith("cost"))
                    {
                        s = s.Substring(4);
                        if (short.TryParse(s, out short n) && n >= 0)
                        {
                            party = party is null ? new Party() : party;
                            party.Cost = n;
                            continue;
                        }
                        else
                        {
                            throw new Exception($"{s} is not a valid cost.");
                        }
                    }
                    if (s.StartsWith("mc"))
                    {
                        s = s.Substring(2);
                        party = party is null ? new Party() : party;
                        party.MysticCode = s;
                        continue;
                    }
                    if (s.StartsWith("ndps"))
                    {
                        s = s.Substring(4);
                        if (byte.TryParse(s, out byte n) && n <= 6 && n >= 1)
                        {
                            nDps = n;
                        }
                        else
                        {
                            throw new Exception($"{s} is not a valid number of dps");
                        }
                        continue;
                    }
                    
                    if (s.StartsWith("s"))
                    {
                        s = s.Substring(1);
                        if (byte.TryParse(s, out byte n) && n <= 6 && n >= 1)
                        {
                            party = party is null ? new Party() : party;
                            if (party.Servants.Exists(x => x.Slot == n))
                            {
                                throw new Exception("The servant slot already exists.");
                            }
                            servant = new Servant(n);
                            party.Servants.Add(servant);
                            continue;
                        }
                        else
                        {
                            throw new Exception($"{s} is not a valid servant slot");
                        }
                    }
                    if (s.StartsWith("id"))
                    {
                        if (servant is null)
                        {
                            byte[] slots = party.Servants.Select(x => x.Slot).ToArray();
                            byte n = 0;
                            for (byte j = 1; j <= 6; ++j)
                            {
                                if (!slots.Contains(j)) n = j;

                            }
                            if (n == 0) throw new Exception("Too many servant slot.");
                            servant = new Servant(n);

                        }
                        s = s.Substring(2);
                        servant.AssignIdentifier(s);
                        continue;

                    }
                    if (s.StartsWith("a"))
                    {
                        if (servant is null) throw new Exception("No servant found.");
                        s = s.Substring(1);
                        if (int.TryParse(s, out int n))
                        {
                            servant.Attack = n;
                            continue;
                        }
                        else
                        {
                            throw new Exception($"{s} is not a valid attack number.");
                        }

                    }
                    if (s.StartsWith("ce"))
                    {
                        if (servant is null) throw new Exception("No servant found.");
                        s = s.Substring(2);

                        servant.CraftEssence = db.GetCeId(s);
                    }
                    if (s.StartsWith("d"))
                    {
                        s = s.Substring(1);
                        if (byte.TryParse(s, out byte n) && n >= 1 && n <= 6)
                        {
                            Servant serv = party.Servants.Where(x => x.Slot == n)?.First();
                            if (serv is null) throw new Exception($"No servent has been assigned to slot {n}.");
                            dirtySlot = n;
                            continue;
                        }
                        else
                        {
                            throw new Exception($"{s} is not a valid servant slot.");
                        }
                    }
                    
                    if (s == "rta")
                    {
                        rta = true;
                        continue;
                    }
                    if (s == "noce")
                    {
                        noCe = true;
                        noCeDps = true;
                        noEventCe = true;
                        continue;
                    }
                    if (s == "noeventce")
                    {
                        noEventCe = true;
                        continue;
                    }
                    if (s == "nocedps")
                    {
                        noCeDps = true;
                        continue;
                    }

                }
            }

            Run run = new Run(quest, runUrl, dps, user)
            {

            };*/
            return null;
        }
        private static Quest GetQuest(RunsContext context, string quest)
        {
            Quest? q = null;
            if (int.TryParse(quest, out int id))
            {
                q = context.Quests.Where(x => x.Id == id).FirstOrDefault();
                if (q is null) throw new Exception($"Quest {quest} could not be found.");
            }
            else
            {
                q = context.QuestAliases.Where(x => x.Nickname == quest).FirstOrDefault()?.Quest;
                if (q is null) throw new Exception($"Quest {quest} could not be found.");

            }
            return q;
        }

        private static Servant GetServant(RunsContext context, string dps)
        {
            Servant? d = null;
            if (int.TryParse(dps, out int id))
            {
                d = context.Servants.Where(x => x.Id == id).FirstOrDefault();
                if (d is null) throw new Exception($"Servant {dps} could not be found.");
            }
            else
            {
                d = context.ServantAliases.Where(x => x.Nickname == dps).FirstOrDefault()?.Servant;
                if (d is null) throw new Exception($"Servant {dps} could not be found.");
            }
            return d;
        }
    }
}
