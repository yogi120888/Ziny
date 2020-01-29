using ChemiFriend.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ChemiFriend.ENTITY
{
    public class Product : BaseEntity
    {
        [Key]
        public Int64 ProductId { get; set; }
        public Int64 ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategories { get; set; }
        public Int64? ProductSubCategoryId { get; set; }
        [ForeignKey("ProductSubCategoryId")]
        public ProductSubCategory ProductSubCategories { get; set; }
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }
        public int ProductTypeId { get; set; }
        public string MarketingCompany { get; set; }
        public decimal MRP { get; set; }
        public decimal PTR { get; set; }
        public int ProductCodeId { get; set; } // Like Molecules
    }
}
