using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class LicenceImages
    {
        [Key]
        public Int64 ImageId { get; set; }
        public string ImagePath { get; set; }
        public Int64 RegistrationId { get; set; }
        public bool IsActive { get; set; } 
    }
}
