using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity.JsonModel
{
    public class GetProductSubCategoryModel
    {
        public Int64 ProductSubCategoryId { get; set; }
        public Int64 ProductCategoryId { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategoryName { get; set; }
        public string ProductSubCategoryImagePath { get; set; }
        public byte Status { get; set; }
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
