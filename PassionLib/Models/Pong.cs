using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public class Pong
    {
        public Pong(string userMention, bool toBePinged = true)
        {
            UserMention = userMention;
            ToBePinged = toBePinged;
        }
        [Key]
        public string UserMention { get; set; }

        public bool ToBePinged { get; set; } = false;
        public int LastSummonCount { get; set; } = 0;
    }
}
