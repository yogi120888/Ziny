using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChemiFriend.Entity
{
    
    public abstract class BaseEntity
    {
        public byte Status { get; set; } = 0;
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
