using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemiFriend.Web.Models.InputModel
{
    public class OrderModel
    {
        public Int64 OrderId { get; set; }
        public Int64 UserId { get; set; } // Customer Id
        public decimal GrandTotal { get; set; }
        public bool PaymentStatus { get; set; }
        public byte Status { get; set; } = 0;
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string PaymentMode { get; set; }
        public List<OrderItemModel> orderItems { get; set; }
    }
}