using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public class Quest : Entity
    {
        public Quest(int id, string jpName) : base(id, jpName)
        {
        }
    }
}
