using ChemiFriend.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChemiFriend.ENTITY
{
    public class ProductCategory: BaseEntity
    {
        [Key]
        public Int64 ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductCategoryImagePath { get; set; }
    }
}
