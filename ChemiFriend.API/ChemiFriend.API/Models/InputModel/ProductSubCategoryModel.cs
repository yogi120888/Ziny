using ChemiFriend.API.Models.ImageUploadModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.API.Models.InputModel
{
    public class ProductSubCategoryModel
    {
        public Int64 ProductSubCategoryId { get; set; }
        [Required]
        public Int64 ProductCategoryId { get; set; }
        [Required]
        public string ProductSubCategoryName { get; set; }
        public string ProductSubCategoryImagePath { get; set; }
        public DocumentModel DocProductSubCategory { get; set; }
        public byte Status { get; set; } 
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; } 
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}