
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class GSTApplicable
    {
        [Key]
        public int GSTApplicableId { get; set; }
        public decimal GST { get; set; }
        public bool Status { get; set; }
    }
}
