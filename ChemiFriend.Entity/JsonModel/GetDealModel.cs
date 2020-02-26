using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.Entity.JsonModel
{
    public class GetDealModel
    {
        public Int64 DealId { get; set; }
        public int DealType { get; set; }
        public string DealTypeName { get; set; }
        public int DealApplicableForId { get; set; }
        public string DealApplicableFor { get; set; }
        public string ProductCategory { get; set; }
        public Int64 ProductSubCategoryId { get; set; }
        public string ProductSubCategory { get; set; }
        public Int64 ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductCodeId { get; set; } // Like Molecules
        public string ProductCode { get; set; } // Like Molecules
        public string ProductImagePath { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public DateTime DealStartDate { get; set; }
        public DateTime DealEndDate { get; set; }
        public DateTime ProductExpiryDate { get; set; }
        public int DealEndDays { get; set; }
        public int DealEndHours { get; set; }
        public string MarketingCompany { get; set; }
        public string Brand { get; set; }
        public int FormTypeId { get; set; }
        public string FormType { get; set; }
        public decimal MRP { get; set; }
        public decimal PTR { get; set; }
        public string Composition { get; set; }
        public int StockAvailable { get; set; }
        public int MaxQuantityForRetailer { get; set; }
        public int PackTypeId { get; set; }
        public string PackType { get; set; }
        public string PackSize { get; set; }
        public int GSTApplicableId { get; set; }
        public decimal GST { get; set; }
        public byte Status { get; set; }
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
      //  public List<GetSchemeModel> lstSchemes { get; set; }
    }
}

