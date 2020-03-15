using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity.JsonModel
{
    public class GetCustomerOrderModel
    {
        public Int64 OrderId { get; set; }
        public Int64 UserId { get; set; } // Customer Id
        public string Retailer { get; set; } // Customer 
        public string RetailerPhone { get; set; }
        public string RetailerCity { get; set; }
        public string RetailerState { get; set; }
        public string ZipCode { get; set; }
        public string RetailerAddress { get; set; }
        public decimal GrandTotal { get; set; }
        public byte PaymentStatus { get; set; }
        public byte Status { get; set; } = 0;
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<GetOrderItemModel> orderItemModels { get; set; }
    }
}
