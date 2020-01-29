using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class Scheme : BaseEntity
    {
        [Key]
        public Int64 SchemeId { get; set; }
        public Int64 DealId { get; set; }
        public int SchemeTypeId { get; set; }
        public int MinOrderQuantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal DealRate { get; set; }
        public decimal Saving { get; set; }
        public string DealScheme { get; set; }
    }
}
