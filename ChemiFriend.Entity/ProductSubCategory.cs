using ChemiFriend.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ChemiFriend.ENTITY
{
    public class ProductSubCategory : BaseEntity
    {
        [Key]
        public Int64 ProductSubCategoryId { get; set; }
        public Int64 ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategories { get; set; }
        public string ProductSubCategoryName { get; set; }
        public string ProductSubCategoryImagePath { get; set; }
    }
}
