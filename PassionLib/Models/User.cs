using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? DiscordUsername { get; set; }
        public string? DiscordDiscriminator { get; set; }
        public long? DiscordSnowflake { get; set; }

    }
}
