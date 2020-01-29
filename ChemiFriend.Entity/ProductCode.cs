using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class ProductCode
    {
        [Key]
        public int ProductCodeId { get; set; }
        public string ProductCodes { get; set; }
        public bool Status { get; set; }
    }
}
