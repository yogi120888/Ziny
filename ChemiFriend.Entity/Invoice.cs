using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class Invoice : BaseEntity
    {
        [Key]
        public Int64 InvoiceId { get; set; }
        public Int64 OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Orders { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceDetails { get; set; }
    }
}
