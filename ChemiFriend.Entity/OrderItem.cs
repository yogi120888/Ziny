using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class OrderItem : BaseEntity
    {
        [Key]
        public Int64 OrderItemId { get; set; }
        public Int64 ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Products { get; set; }
        public Int64 OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Orders { get; set; }
        public int OrderItemQuantity { get; set; }
        public decimal OrderItemPrice { get; set; }
        public DateTime OrderIssuedDate { get; set; }
    }
}
