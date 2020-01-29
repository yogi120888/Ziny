using ChemiFriend.API.Models.ImageUploadModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.API.Models.InputModel
{
    public class ProductModel
    {
        public Int64 ProductId { get; set; }
        [Required]
        public Int64 ProductCategoryId { get; set; }
        public Int64? ProductSubCategoryId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }
        public DocumentModel DocProduct { get; set; }
        [Required]
        public int ProductTypeId { get; set; }
        public string MarketingCompany { get; set; }
        [Required]
        public decimal MRP { get; set; }
        [Required]
        public decimal PTR { get; set; }
        [Required]
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