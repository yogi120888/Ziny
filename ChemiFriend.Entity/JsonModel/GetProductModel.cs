using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity.JsonModel
{
    public class GetProductModel
    {
        public Int64 ProductId { get; set; }
        public Int64 ProductCategoryId { get; set; }
        public string ProductCategory { get; set; }
        public Int64? ProductSubCategoryId { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public string MarketingCompany { get; set; }
        public decimal MRP { get; set; }
        public decimal PTR { get; set; }
        public int ProductCodeId { get; set; }
        public string ProductCodes { get; set; }
        public byte Status { get; set; }
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
