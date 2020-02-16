
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.Web.Models.InputModel
{
    public class ProductCodeModel
    {
        public int ProductCodeId { get; set; }
        [Required]
        public string ProductCodes { get; set; }
        public bool Status { get; set; }
    }
}