using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity.JsonModel
{
    public class GetOrderItemModel
    {
        public Int64 OrderItemId { get; set; }
        public Int64 OrderId { get; set; }
        public Int64 UserId { get; set; } // Customer Id
        public string Retailer { get; set; } // Customer
        public Int64 DealId { get; set; }
        public Int64 ProductId { get; set; }
        public string Product { get; set; }
        public Int64 ProductSubCategoryId { get; set; }
        public string Brand { get; set; }
        public Int64 ProductCategoryId { get; set; }
        public string Company { get; set; }
        public Int64 SchemeId { get; set; }
        public string Scheme { get; set; }
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
