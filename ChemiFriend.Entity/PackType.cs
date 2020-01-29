using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class PackType
    {
        [Key]
        public int PackTypeId { get; set; }
        public string PackTypeName { get; set; }
        public bool Status { get; set; }
    }
}
