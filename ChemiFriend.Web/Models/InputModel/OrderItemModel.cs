using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemiFriend.Web.Models.InputModel
{
    public class OrderItemModel
    {
        public Int64 OrderItemId { get; set; }
        public Int64 UserId { get; set; } // Customer Id
        public Int64 DealId { get; set; }
        public Int64 ProductId { get; set; }
        public Int64 SchemeId { get; set; }
        public int OrderItemQuantity { get; set; }
        public decimal OrderItemUnitPrice { get; set; }
        public string OrderItemSchemes { get; set; }
        public byte Status { get; set; } = 0;
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}