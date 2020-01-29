using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class DealApplicableFor
    {
        [Key]
        public int DealApplicableForId { get; set; }
        public string Applicable { get; set; }
        public bool Status { get; set; }
    }
}
