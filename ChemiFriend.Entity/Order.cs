using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class Order : BaseEntity
    {
        [Key]
        public Int64 OrderId { get; set; }
        public Int64 UserId { get; set; } // Customer Id
        public decimal GrandTotal { get; set; }
        public byte PaymentStatus { get; set; }
        [ForeignKey("OrderId")]
        public List<OrderItem> orderItems { get; set; }
    }
}
