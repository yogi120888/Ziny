using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class Shipment : BaseEntity
    {
        [Key]
        public Int64 ShipmentId { get; set; }
        public Int64 OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Orders { get; set; }
        public Int64 InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoices { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string ShipmentDetails { get; set; }
    }
}
