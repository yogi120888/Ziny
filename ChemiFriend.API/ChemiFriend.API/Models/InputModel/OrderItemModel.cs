using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.API.Models.InputModel
{
    public class OrderItemModel
    {
        public Int64 OrderItemId { get; set; }
        [Required]
        public Int64 ProductId { get; set; }
        [Required]
        public Int64 OrderId { get; set; }
        [Required]
        public int OrderItemQuantity { get; set; }
        [Required]
        public decimal OrderItemPrice { get; set; }
        [Required]
        public DateTime OrderIssuedDate { get; set; }
    }
}