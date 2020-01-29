using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class Deal : BaseEntity
    {
        [Key]
        public Int64 DealId { get; set; }
        public int DealType { get; set; }
        public int DealApplicableFor { get; set; }
        public Int64 ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public DateTime DealStartDate { get; set; }
        public DateTime DealEndDate { get; set; }
        public DateTime ProductExpiryDate { get; set; }
        public string MarketingCompany { get; set; }
        public string Brand { get; set; }
        public int FormType { get; set; }
        public decimal MRP { get; set; }
        public decimal PTR { get; set; }
        public string Composition { get; set; }
        public int? StockAvailable { get; set; }
        public int? MaxQuantityForRetailer { get; set; }
        public int PackType { get; set; }
        public string PackSize { get; set; }
        public int GST { get; set; }
        public string ProductImagePath { get; set; }
       // public List<Scheme> lstSchemes { get; set; }
    }
}
