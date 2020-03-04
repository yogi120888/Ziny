using ChemiFriend.Web.Models.ImageUploadModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.Web.Models.InputModel
{
    public class DealModel
    {
        public Int64 DealId { get; set; }
        [Required]
        public int DealType { get; set; }
        [Required]
        public int DealApplicableFor { get; set; }
        [Required]
        public Int64 ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
        [Required]
        public DateTime DealStartDate { get; set; }
        [Required]
        public DateTime DealEndDate { get; set; }
        [Required]
        public DateTime ProductExpiryDate { get; set; }
        public string MarketingCompany { get; set; }
        public string Brand { get; set; }
        [Required]
        public int FormType { get; set; }
        [Required]
        public decimal MRP { get; set; }
        [Required]
        public decimal PTR { get; set; }
        public string Composition { get; set; }
        public int? StockAvailable { get; set; }
        public int? MaxQuantityForRetailer { get; set; }
        [Required]
        public int PackType { get; set; }
        public string PackSize { get; set; }
        [Required]
        public int GST { get; set; }
        public int? ApplicableTaxType { get; set; } // DPCO/NONDPCO
        public byte Status { get; set; }
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ProductImagePath { get; set; }
        public DocumentModel DocProduct { get; set; }
        public List<SchemeModel> lstSchemes { get; set; } 
    }
}