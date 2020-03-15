using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity.JsonModel
{
    public class OrderDetailModel
    {
        public Int64 UserId { get; set; }
        public Int64 DealId { get; set; }
        public Int64 ProductId { get; set; }
        public string Product { get; set; }
        public string ProductImagePath { get; set; }
        public Int64 Quantity { get; set; }
        public Int64 SchemeId { get; set; }
        public string Scheme { get; set; }
        public decimal DealPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
