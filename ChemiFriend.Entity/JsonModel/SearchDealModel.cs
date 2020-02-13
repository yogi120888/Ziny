using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity.JsonModel
{
    public class SearchDealModel : PaginationModel
    {
        public string Search { get; set; } 
        public string DealType { get; set; } 
        public string Sort { get; set; } 
        public List<GetDealModel> getDealModels { get; set; }
    }
}
