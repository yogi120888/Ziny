using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity.JsonModel
{
    public class GetSchemeWithDealModel
    {
        public Int64 SchemeId { get; set; }
        public Int64 DealId { get; set; }
        public string SchemeType { get; set; }
        public int MinOrderQuantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal DealRate { get; set; }
        public decimal Saving { get; set; }
        public string DealScheme { get; set; }
        // add Deal Model
       // public string ProductSubCategory { get; set; } // Brand
        public string ProductCategory { get; set; }
        public Int64 ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }
        public string ProductType { get; set; }
        public DateTime DealStartDate { get; set; }
        public DateTime DealEndDate { get; set; }
        public DateTime ProductExpiryDate { get; set; }
        public string MarketingCompany { get; set; }
        public string Brand { get; set; }
        public string FormType { get; set; }
        public decimal MRP { get; set; }
        public decimal PTR { get; set; }
        public string Composition { get; set; }
        public int StockAvailable { get; set; }
        public string PackType { get; set; }
        public string PackSize { get; set; }
        public byte Status { get; set; }
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
