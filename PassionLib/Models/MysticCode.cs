using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public class MysticCode : Entity
    {
        public MysticCode(int id, string jpName) : base(id, jpName)
        {
        }
    }
}
