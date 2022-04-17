using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public class CerealShrine
    {
        [Key]
        public int Id { get; set; }
        public int Prayers { get; set; }
        public Pong? LastPong { get; set; }
        public bool SendPrayer(Pong p)
        {
            LastPong = p;
            Prayers++;
            if(Prayers >= 29)
            {
                Prayers = 0;
                return true;
            }
            return false;
        }
        public CerealShrine()
        {
            Prayers = 0;
        }
    }
}
