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

            List<PartySlot>? p = party;

            Run run = new Run(quest, runUrl, dps, user);
            if (p is not null)
            {

                run.Party = p;
                //ask if craft essence/servant are null
            }


            //Do not open unless you want suicidal thoughts 
            if (args is not null && !args.Any())
            {
                for (int i = 0; i < args.Length; ++i)
                {
                    Quest servant = null;
                    string s = args[i].ToLower();
                    if (s == "f")
                    {
                        run.Failure = true;
                        continue;
                    }
                    if (s.StartsWith("cs"))
                    {
                        s = s.Substring(2);
                        if (short.TryParse(s, out short n))
                        {
                            run.CsUsed = n;
                        }
                        else
                        {
                            run.CsUsed = 1;
                        }
                        continue;
                    }
                    if (s.StartsWith("rev"))
                    {
                        s = s.Substring(3);
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
                            run.Cost = n;
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
                        MysticCode? mc = null;
                        if (int.TryParse(s, out int n) && n >= 0)
                        {
                            mc = context.MysticCodes.FirstOrDefault(x => x.Id == n);
                        }
                        else
                        {
                            mc = context.MysticCodeAliases.FirstOrDefault(x => x.Nickname == s)?.MysticCode;
                        }
                        if (mc is null) throw new Exception($"{s} could not be recognized as a mystic code id or nickname.");
                        run.MysticCode = mc;
                        continue;
                    }
                    if (s.StartsWith("n"))
                    {
                        s = s.Substring(1);
                        if (short.TryParse(s, out short n) && n >= 1 && n <= 6)
                        {
                            run.ServantCount = n;
                        }
                        else throw new Exception($"{s} is not a valid servant count.");
                    }
                    if (s == "rta")
                    {
                        run.Rta = true;
                        continue;
                    }
                    if (s == "noce")
                    {
                        run.NoCe = true;
                        run.NoCeDps = true;
                        run.NoEventCeDps = true;
                        continue;
                    }
                    if (s == "nocedps")
                    {
                        run.NoCeDps = true;
                        run.NoEventCeDps = true;
                        continue;
                    }
                    if (s == "noeventce")
                    {
                        run.NoEventCeDps = true;
                        continue;
                    }
                    if (p is null)
                    {
                        PartySlot? ps = null;
                        if (s.StartsWith("svt"))
                        {
                            p = p is null ? new List<PartySlot>() : p;
                            s = s.Substring(3);
                            if (short.TryParse(s, out short n) && n <= 6 && n >= 1)
                            {
                                if (p.Any(x => x.Slot == n)) throw new Exception("The servant slot already exists.");
                                ps = new PartySlot() { Slot = n };
                                p.Add(ps);

                                continue;
                            }
                            else if (s == "")
                            {
                                //takes the left most slot possible
                                p = p.OrderBy(x => x.Slot).ToList();
                                short temp = 0;
                                for (short j = 0; i < p.Count; i++)
                                {
                                    if (j == p[j].Slot) continue;
                                    else temp = j;
                                }
                                if (temp == 0) temp = (short)p.Count();
                                ps = new PartySlot() { Slot = temp };
                            }
                            else
                            {
                                throw new Exception($"{s} is not a valid servant slot");
                            }
                        }
                        if (ps is not null)
                        {
                            if (s.StartsWith("id"))
                            {
                                if (ps.Servant is not null) throw new Exception($"A servant has already been entered for that party slot.");
                                s = s.Substring(2);
                                Servant? svt = null;
                                if (int.TryParse(s, out int n) && n > 0)
                                {
                                    svt = context.Servants.FirstOrDefault(x => x.Id == n);
                                }
                                else
                                {
                                    svt = context.ServantAliases.FirstOrDefault(x => x.Nickname == s)?.Servant;
                                }
                                if (svt is null) throw new Exception($"{s} could not be recognized as a servant id or nickname.");
                                ps.Servant = svt;

                                continue;
                            }
                            if (s.StartsWith("ce"))
                            {
                                if (ps.CraftEssence is not null) throw new Exception($"A craft essence has already been entered for that party slot.");
                                s = s.Substring(2);
                                CraftEssence? ce = null;
                                if (int.TryParse(s, out int n) && n > 0)
                                {
                                    ce = context.CraftEssences.FirstOrDefault(x => x.Id == n);
                                }
                                else
                                {
                                    ce = context.CraftEssenceAliases.FirstOrDefault(x => x.Nickname == s)?.CraftEssence;
                                }
                                if (ce is null) throw new Exception($"{s} could not be recognized as a craft essence id or nickname.");
                                ps.CraftEssence = ce;

                            }
                            if (s == "mlb")
                            {
                                ps.CraftEssenceMlb = true;
                            }
                            //sets total attack of this partyslot, irrelevant of the servant and ce attack
                            if (s.StartsWith("ta"))
                            {
                                s = s.Substring(2);
                                if (short.TryParse(s, out short n) && n > 0)
                                {
                                    ps.TotalAttack = n;
                                    continue;
                                }
                                else
                                {
                                    throw new Exception($"{s} is not a valid attack number.");
                                }
                            }
                        }


                        if (s.StartsWith("d"))
                        {
                            s = s.Substring(1);
                            if (byte.TryParse(s, out byte n) && n >= 1 && n <= 6)
                            {
                                
                            }
                            else
                            {
                                throw new Exception($"{s} is not a valid servant slot.");
                            }
                        }
                    }
                }
                if (p is not null && p.Any())
                {
                    //if craft essence is null, sets mlb to null
                    p.ForEach(x => x.CraftEssenceMlb = x.CraftEssence is null ? null : x.CraftEssenceMlb);
                }

            }

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
