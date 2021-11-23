using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public abstract class CollectionEntity : Entity
    {
        public short? Rarity { get; set; }
        public short? BaseMaxAttack { get; set; }
        public int CollectionNo { get; set; }
        protected CollectionEntity(int id, string jpName) : base(id, jpName)
        {
        }
    }
}
