using DSharpPlus.CommandsNext;
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
                bool dirtyCeCheck = false;
                PartySlot? ps = null;
                CollectionEntity? typeCheck = null;
                for (int i = 0; i < args.Length; ++i)
                {

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
                    if (party is null)
                    {
                        if (s.StartsWith("svt"))
                        {
                            dirtyCeCheck = false;
                            p = p is null ? new List<PartySlot>() : p;
                            if (p.Count >= 6) throw new Exception("Cannot have seven servants.");
                            s = s.Substring(3);
                            ps = new PartySlot();
                            Servant? svt = null;
                            if (string.IsNullOrEmpty(s) || s == "null")
                            {
                                ps.Servant = null;
                                continue;
                            }
                            else if (int.TryParse(s, out int n) && n >= 1)
                            {
                                svt = context.Servants.FirstOrDefault(x => x.Id == n || x.CollectionNo == n);
                            }
                            else svt = context.ServantAliases.FirstOrDefault(x => x.Nickname == s)?.Servant;
                            if (svt is null) throw new Exception($"{s} not recognized as servant.");
                            ps.Servant = svt;
                            typeCheck = svt;

                            continue;
                        }
                        if (ps is not null)
                        {
                            if (s.StartsWith("ce") && !dirtyCeCheck)
                            {
                                dirtyCeCheck = true;
                                if (ps.CraftEssence is not null) throw new Exception($"A craft essence has already been entered for that party slot.");
                                s = s.Substring(2);
                                CraftEssence? ce = null;
                                if (string.IsNullOrEmpty(s) || s == "null")
                                {
                                    ps.CraftEssence = null;
                                    p.Add(ps);
                                    ps = null;

                                    continue;
                                }
                                else if (int.TryParse(s, out int n) && n > 0)
                                {
                                    ce = context.CraftEssences.FirstOrDefault(x => x.Id == n);
                                }
                                else ce = context.CraftEssenceAliases.FirstOrDefault(x => x.Nickname == s)?.CraftEssence;

                                if (ce is null) throw new Exception($"{s} could not be recognized as a craft essence id or nickname.");

                                ps.CraftEssence = ce;
                                typeCheck = ce;
                                p.Add(ps);

                            }
                            if (s.StartsWith("l"))
                            {
                                s = s.Substring(1);
                                if (short.TryParse(s, out short n) && n > 0)
                                {
                                    if(typeCheck is Servant && n < 121)
                                    {
                                        ps.ServantLevel = 121;
                                    }
                                    if(typeCheck is CraftEssence && n < 101)
                                    {
                                        ps.CraftEssenceLevel = n;
                                    }
                                    typeCheck = null;
                                }
                                else throw new Exception($"{s} is an invalid level.");

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
                    }
                }
                if (p is not null && p.Any())
                {
                    //if craft essence is null, sets mlb to null
                    p.ForEach(x => x.CraftEssenceMlb = x.CraftEssence is null ? null : x.CraftEssenceMlb);
                    p.ForEach(x => x.TotalAttack = GetAttack(x));
                }

            }

            return null;
        }
        private static short? GetCost(List<PartySlot> l)
        {
            if (l is null || l.Count != 6)
            {
                return null;
            }
            int? cost = 0;
            foreach (var ps in l)
            {
                cost += ps.Servant?.Rarity is not null ? ServantCost[(short)ps.Servant.Rarity] : null;
                cost += ps.CraftEssence?.Rarity is not null ? ServantCost[(short)ps.CraftEssence.Rarity] : 0;
            }
            return (short?)cost;
        }
        private static short? GetAttack(PartySlot p)
        {
            int? atk = null;
            if (p.TotalAttack is not null) atk = p.TotalAttack;
            else if (p.Servant is not null && p.CraftEssence is not null)
            {
                atk += p.ServantLevel is not null ? p.Servant.AttackScaling?[(int)p.ServantLevel] : p.Servant.BaseMaxAttack;
                atk += p.CraftEssenceLevel is not null ? p.CraftEssence.AttackScaling?[(int)p.CraftEssenceLevel] : p.CraftEssence.BaseMaxAttack;

            }
            return (short?)atk;
        }
        private static Dictionary<short, short> ServantCost = new Dictionary<short, short>()
        {
            {5,15 },
            {4,11 },
            {3,6 },
            {2,4 },
            {1,3 },
            {0,4 }
        };
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
        public static User GetUser(CommandContext ctx, RunsContext Context)
        {
            User? user = null;
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
            if (user is null)
            {
                user = new User()
                {
                    DiscordDiscriminator = discordUser.Discriminator,
                    DiscordSnowflake = (long)discordUser.Id,
                    DiscordUsername = discordUser.Username
                };
                Context.Users.Add(user);
            }
            return user;
        }
    }
}
