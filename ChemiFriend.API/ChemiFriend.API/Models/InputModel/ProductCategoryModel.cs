using ChemiFriend.API.Models.ImageUploadModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.API.Models.InputModel
{
    public class ProductCategoryModel
    {
        public Int64 ProductCategoryId { get; set; }
        [Required]
        public string ProductCategoryName { get; set; }
        public string ProductCategoryImagePath { get; set; }
        public DocumentModel DocProductCategory { get; set; }
        public byte Status { get; set; } 
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}