using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public abstract class Alias
    {
        protected Alias(string nickname) => Nickname = nickname;
        [Key]
        public string Nickname { get; set; }
        public virtual User Submitter { get; set; }
    }

    public class CraftEssenceAlias : Alias
    {
        private CraftEssenceAlias(string nickname) : base(nickname)
        {
        }
        public CraftEssenceAlias(CraftEssence craftEssence, string nickname) : base(nickname) => CraftEssence = craftEssence;
        public virtual CraftEssence CraftEssence { get; set; }
    }

    public class QuestAlias : Alias
    {
        private QuestAlias(string nickname) : base(nickname)
        {
        }
        public QuestAlias(Quest quest, string nickname) : base(nickname) => Quest = quest;
        public virtual Quest Quest { get; set; }
    }

    public class MysticCodeAlias : Alias
    {
        private MysticCodeAlias(string nickname) : base(nickname)
        {
        }
        public MysticCodeAlias(MysticCode mysticCode, string nickname) : base(nickname) => MysticCode = mysticCode;
        public virtual MysticCode MysticCode { get; set; }
    }

    public class ServantAlias : Alias
    {
        private ServantAlias(string nickname) : base(nickname)
        {
        }
        public ServantAlias(Servant servant, string nickname) : base(nickname) => Servant = servant;
        public virtual Servant Servant { get; set; }
    }
}
