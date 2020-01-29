using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class SchemeType
    {
        [Key]
        public int SchemeTypeId { get; set; }
        public string SchemeTypeName { get; set; }
        public bool Status { get; set; }
    }
}
