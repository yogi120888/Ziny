using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class DealType
    {
        [Key]
        public int DealTypeId { get; set; }
        public string DealTypeName { get; set; }
        public bool Status { get; set; }
    }
}
