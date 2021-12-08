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
        public Pong(string userMention) => UserMention = userMention;
        [Key]
        public string UserMention { get; set; }
    }
}
