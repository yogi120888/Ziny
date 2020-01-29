
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class Payment : BaseEntity
    {
        [Key]
        public Int64 PaymentId { get; set; }
        public Int64 InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoices { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
