﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.Entity.JsonModel
{
    public class GetSchemeModel
    {
        public Int64 SchemeId { get; set; }
        public Int64 DealId { get; set; }
        public int SchemeTypeId { get; set; }
        public string SchemeType { get; set; }
        public int MinOrderQuantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal DealRate { get; set; }
        public decimal Saving { get; set; }
        public string DealScheme { get; set; }
        public byte Status { get; set; }
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; }
        public string User { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}