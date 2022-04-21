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
        
        public Pongv2? LastPong { get; set; }
        public int Countdown { get; set; } = 29;
        public bool SendPrayer(Pongv2 p)
        {
            LastPong = p;
            Prayers++;
            if (Prayers >= 29)
            {
                Prayers = 0;
                return true;
            }
            return false;
        }
        public int LowerCountdown()
        {
            Countdown--;
            return Countdown;
        }
        public CerealShrine()
        {
            Prayers = 0;
        }
    }
}
