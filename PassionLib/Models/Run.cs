﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public class Run
    {
        //I think only this one should be used
        public Run(Quest quest, string runUrl, Servant servant, User submitter) : this(quest, runUrl)
        {
            Submitter = submitter;
            Dps = servant;
        }
        public Run(Quest quest, string runUrl) : this(runUrl) => Quest = quest;
        private Run(string runUrl) => RunUrl = runUrl;
        // EF complains if there's no reference-less constructor, see https://stackoverflow.com/a/55750607

        [JsonIgnoreAttribute]
        public int Id { get; set; }
        public virtual Quest Quest { get; set; }
        public short? Phase { get; set; }

        public virtual Servant Dps { get; set; }
        public virtual List<PartySlot> Party { get; set; } = new List<PartySlot>();
        public virtual MysticCode? MysticCode { get; set; }
        public DateTime? RunDate { get; set; }
        public string RunUrl { get; set; }
        public int? CsUsed { get; set; }
        public int? RevivesUsed { get; set; }
        public bool? Failure { get; set; } = false;
        public bool? Rta { get; set; } = false;
        public string? Misc { get; set; }

        //These can be inferred from party or entered through argument
        public short? Cost { get; set; }//through image recognition or adding costs from each servant
        public short? ServantCount { get; set; } = 6;
        public bool? NoCe { get; set; } = false;
        public bool? NoCeDps { get; set; } = false;
        public bool? NoEventCeDps { get; set; } = false;
        public bool? NoDupe { get; set; } = false;

        [JsonIgnoreAttribute]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnoreAttribute]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnoreAttribute]
        public virtual User? Submitter { get; set; }
    }

    [Owned]
    public class PartySlot
    {
        [Key]
        public int Id { get; set; }
        public virtual Servant? Servant { get; set; } = null;
        public virtual CraftEssence? CraftEssence { get; set; } = null;
        public bool? CraftEssenceMlb { get; set; } = null;
        public short? Slot { get; set; } = null;
        public short? TotalAttack { get; set; } = null;
    }
}
