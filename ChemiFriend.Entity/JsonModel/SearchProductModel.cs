using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity.JsonModel
{
    public class SearchProductModel
    {
        public Int64 SubCategoryId { get; set; }
        public string Search { get; set; }
        public List<GetProductModel> getProductModel { get; set; }
    }
}
