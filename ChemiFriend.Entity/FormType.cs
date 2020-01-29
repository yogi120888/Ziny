using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class FormType
    {
        [Key]
        public int FormTypeId { get; set; }
        public string FormTypeName { get; set; }
        public bool Status { get; set; }
    }
}
