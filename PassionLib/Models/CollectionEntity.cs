using Newtonsoft.Json;
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
        [JsonIgnoreAttribute]
        public short[]? AttackScaling { get; set; }
        public short? Cost { get; set; }
        public int CollectionNo { get; set; }
        protected CollectionEntity(int id, string jpName, int collectionNo) : base(id, jpName)
        {
            CollectionNo = collectionNo;
        }
    }
}
