using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PassionLib.Models
{
    public abstract class Entity
    {
        protected Entity(int id, string jpName)
        {
            Id = id;
            JpName = jpName;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string JpName { get; set; }
        public string? NaName { get; set; }
        public string? TwName { get; set; }
        public string? CnName { get; set; }
        public string? KrName { get; set; }
    }
}
