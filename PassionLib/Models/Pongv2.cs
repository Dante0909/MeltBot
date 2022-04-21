using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public class Pongv2
    {
        public Pongv2(ulong id, bool toBePinged = true, int lastSummonCount = 0)
        {
            Id = id;
            ToBePinged = toBePinged;
            LastSummonCount = lastSummonCount;
        }
        [System.ComponentModel.DataAnnotations.Key]
        public ulong Id { get; set; }
        public bool? ToBePinged { get; set;}
        public int? LastSummonCount { get; set; }
        public string UserMention()
        {
            return $"<@{Id}>";
        }
    }
}
