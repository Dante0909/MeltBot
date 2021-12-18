using DSharpPlus.CommandsNext;
using Microsoft.EntityFrameworkCore;
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
        public static Run CreateRun(RunsContext context, Quest q, string runUrl, User user, List<PartySlot>? party, Run? r, params string[] args)
        {
            if (runUrl.StartsWith("<")) runUrl = runUrl.Substring(1);
            if (runUrl.EndsWith(">")) runUrl = runUrl.Substring(0, runUrl.Length - 1);
            if (!Uri.IsWellFormedUriString(runUrl, UriKind.Absolute)) throw new CustomException($"Invalid Url format : <{runUrl}>");

            List<PartySlot>? p = party;

            if (r is null)
            {
                if (context.Runs.Where(x => x.RunUrl == runUrl).Any()) throw new CustomException("A run with the provided link already exists");
                r = new Run()
                {
                    Quest = q,
                    RunUrl = runUrl,
                    Submitter = user
                };
            }
            else
            {
                r.CsUsed = null;
                r.RevivesUsed = null;
                r.Failure = false;
                r.Rta = false;
                r.Cost = null;
                r.ServantCount = null;
                r.NoCe = null;
                r.NoCeDps = null;
                r.NoEventCeDps = null;
                r.NoDupe = null;
                r.Party = null;
                r.MysticCode = null;
                
            }
            if (p is not null)
            {
                r.Party = p;

                //ask if craft essence/servant are null
            }


            //Do not open unless you want suicidal thoughts 
            if (args is not null && args.Any())
            {
                bool dirtyCeCheck = false;
                PartySlot? ps = null;
                CollectionEntity? typeCheck = null;
                for (int i = 0; i < args.Length; ++i)
                {

                    string s = args[i].ToLower();
                    if (s == "f")
                    {
                        r.Failure = true;
                        continue;
                    }
                    if (s.StartsWith("cs"))
                    {
                        s = s.Substring(2);
                        if (short.TryParse(s, out short n))
                        {
                            r.CsUsed = n;
                        }
                        else
                        {
                            r.CsUsed = 1;
                        }
                        continue;
                    }
                    if (s.StartsWith("rev"))
                    {
                        s = s.Substring(3);
                        if (short.TryParse(s, out short n))
                        {
                            r.RevivesUsed = n;
                        }
                        else
                        {
                            r.RevivesUsed = 1;
                        }
                        continue;
                    }
                    if (s.StartsWith("cost"))
                    {
                        s = s.Substring(4);
                        if (short.TryParse(s, out short n) && n >= 0)
                        {
                            r.Cost = n;
                            continue;
                        }
                        else
                        {
                            throw new CustomException($"{s} is not a valid cost");
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
                            mc = context.MysticCodeAliases.Include(x => x.MysticCode).FirstOrDefault(x => x.Nickname == s)?.MysticCode;
                        }
                        if (mc is null) throw new CustomException($"{s} could not be recognized as a mystic code id or nickname");
                        r.MysticCode = mc;
                        continue;
                    }
                    if (s.StartsWith("n"))
                    {
                        s = s.Substring(1);
                        if (short.TryParse(s, out short n) && n >= 1 && n <= 6)
                        {
                            r.ServantCount = n;
                        }
                        else throw new CustomException($"{s} is not a valid servant count");
                    }
                    if (s == "rta")
                    {
                        r.Rta = true;
                        continue;
                    }
                    if (s == "noce")
                    {
                        r.NoCe = true;
                        r.NoCeDps = true;
                        r.NoEventCeDps = true;
                        continue;
                    }
                    if (s == "nocedps")
                    {
                        r.NoCeDps = true;
                        r.NoEventCeDps = true;
                        continue;
                    }
                    if (s == "noeventce")
                    {
                        r.NoEventCeDps = true;
                        continue;
                    }
                    if(s == "solo")
                    {
                        r.Solo = true;
                        continue;
                    }
                    if (party is null)
                    {
                        if (s.StartsWith("s"))
                        {
                            dirtyCeCheck = false;
                            p = p is null ? new List<PartySlot>() : p;
                            if (p.Count >= 6) throw new CustomException("Cannot have seven servants");
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
                            else svt = context.ServantAliases.Include(x => x.Servant).FirstOrDefault(x => x.Nickname == s)?.Servant;
                            if (svt is null) throw new CustomException($"{s} not recognized as servant");
                            ps.Servant = svt;
                            typeCheck = svt;

                            continue;
                        }
                        if (ps is not null)
                        {
                            if (s.StartsWith("ce") && !dirtyCeCheck)
                            {
                                dirtyCeCheck = true;
                                if (ps.CraftEssence is not null) throw new CustomException($"A craft essence has already been entered for that party slot");
                                s = s.Substring(2);
                                CraftEssence? ce = null;
                                if (string.IsNullOrEmpty(s) || s == "null")
                                {
                                    ps.CraftEssence = null;
                                    p.Add(ps);

                                    continue;
                                }
                                else if (int.TryParse(s, out int n) && n > 0)
                                {
                                    ce = context.CraftEssences.FirstOrDefault(x => x.Id == n || x.CollectionNo == n);
                                }
                                else ce = context.CraftEssenceAliases.Include(x => x.CraftEssence).FirstOrDefault(x => x.Nickname == s)?.CraftEssence;

                                if (ce is null) throw new CustomException($"{s} could not be recognized as a craft essence id or nickname");

                                ps.CraftEssence = ce;
                                typeCheck = ce;
                                p.Add(ps);

                            }
                            if (s.StartsWith("l"))
                            {
                                s = s.Substring(1);
                                if (short.TryParse(s, out short n) && n > 0)
                                {
                                    if (typeCheck is Servant && n < 121)
                                    {
                                        ps.ServantLevel = n;
                                    }
                                    if (typeCheck is CraftEssence && n < 101)
                                    {
                                        ps.CraftEssenceLevel = n;
                                    }
                                    typeCheck = null;
                                }
                                else throw new CustomException($"{s} is an invalid level");

                            }
                            if (s.StartsWith("fou"))
                            {
                                s = s.Substring(3);
                                if (short.TryParse(s, out short n) && n >= 0 && n <= 2000)
                                {
                                    ps.ServantFou = n;
                                }
                                else throw new CustomException($"{s} is an invalid fou");

                            }
                            if (s == "main")
                            {
                                r.Dps = ps;
                                ps.IsMainDps = true;
                            }
                            if (s == "b")
                            {
                                ps.Borrowed = true;
                            }
                            if (s == "mlb")
                            {
                                ps.CraftEssenceMlb = false;
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
                                    throw new CustomException($"{s} is not a valid attack number");
                                }
                            }
                        }
                    }
                }

                if (p is not null && p.Any())
                {
                    if (p.Count == 6)
                    {
                        if (!p.Any(x => x.Borrowed == true)) throw new CustomException("No servant is borrowed");
                        
                        r.Cost = r.Cost is not null ? r.Cost : GetCost(p);
                        r.ServantCount = r.ServantCount is not null ? r.ServantCount : (short)p.Count(x => x.Servant is not null);
                        r.NoCe = !p.Any(x => x.CraftEssence is not null);
                        r.NoDupe = !p.GroupBy(x => x.Servant?.Id).Any(c => c.Count() > 1);
                    }
                    else if (p.Count == 1)
                    {

                    }
                    else throw new CustomException("Enter one or six servants in the party");
                    
                    if (r.Dps is null)
                    {
                        p.First().IsMainDps = true;
                        r.Dps = p.First();
                    }
                    //if craft essence is null, sets mlb to null
                    p.ForEach(x => x.CraftEssenceMlb = x.CraftEssence is null ? null : x.CraftEssenceMlb);
                    p.ForEach(x => x.TotalAttack = GetAttack(x));

                    r.NoCeDps = r.Dps.CraftEssence is null;

                    r.Party = p;
                }
                else throw new CustomException("No servant in the party");

            }

            return r;
        }
        private static short? GetCost(List<PartySlot> l)
        {
            if (l is null || l.Count != 6)
            {
                return null;
            }
            int? totalCost = 0;
            foreach (var ps in l)
            {
                if (ps.Borrowed == false)
                {
                    int? cost = 0;
                    if (ps.Servant is not null)
                    {
                        cost += ps.Servant.Cost;
                    }

                    if (ps.CraftEssence is not null)
                    {
                        cost += ps.CraftEssence.Cost;
                    }
                    totalCost += cost;
                }

            }
            return (short?)totalCost;
        }
        private static short? GetAttack(PartySlot p)
        {
            int? atk = 0;
            if (p.TotalAttack is not null) atk = p.TotalAttack;
            else
            {
                if (p.Servant is not null)
                {
                    atk += p.ServantLevel is not null ? p.Servant.AttackScaling?[(int)p.ServantLevel - 1] : p.Servant.BaseMaxAttack;
                    atk += p.ServantFou;
                }

                if (p.CraftEssence is not null) atk += p.CraftEssenceLevel is not null ? p.CraftEssence.AttackScaling?[(int)p.CraftEssenceLevel - 1] : p.CraftEssence.BaseMaxAttack;
            }
            return (short?)atk;
        }

        public static Quest GetQuest(RunsContext context, string quest)
        {
            Quest? q = null;
            if (int.TryParse(quest, out int id))
            {
                q = context.Quests.FirstOrDefault(x => x.Id == id);
                if (q is null) throw new CustomException($"Quest {quest} could not be found");
            }
            else
            {
                q = context.QuestAliases.Where(x => x.Nickname == quest).Include(x => x.Quest).FirstOrDefault()?.Quest;
                if (q is null) throw new CustomException($"Quest {quest} could not be found");

            }
            return q;
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
