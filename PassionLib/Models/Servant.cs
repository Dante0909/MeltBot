using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public class Servant : CollectionEntity
    {
        public string? Class { get; set; }
        public Servant(int id, string jpName, int collectionNo) : base(id, jpName, collectionNo)
        {
        }
    }
}
