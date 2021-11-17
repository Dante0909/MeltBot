using Microsoft.EntityFrameworkCore;
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
        public Run(Quest quest, string runUrl, User submitter) : this(quest, runUrl)
        {
            Submitter = submitter;

        }
        public Run(Quest quest, string runUrl) : this(runUrl) => Quest = quest;
        private Run(string runUrl) => RunUrl = runUrl;
        // EF complains if there's no reference-less constructor, see https://stackoverflow.com/a/55750607

        [JsonIgnoreAttribute]
        public int Id { get; set; }
        public virtual Quest Quest { get; set; }
        public short? Phase { get; set; }

        public short Dps { get; set; }
        public virtual List<PartySlot> Party { get; set; } = new List<PartySlot>();
        public virtual MysticCode? MysticCode { get; set; }
        public DateTime? RunDate { get; set; }
        public string RunUrl { get; set; }
        public int? CsUsed { get; set; }
        public int? RevivesUsed { get; set; }
        public string? Misc { get; set; }
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
        public virtual Servant? Servant { get; set; }
        public virtual CraftEssence? CraftEssence { get; set; }
        public bool? CraftEssenceMlb { get; set; }
    }
}
