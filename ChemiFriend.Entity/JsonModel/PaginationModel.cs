using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity.JsonModel
{
    public class PaginationModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecordCount { get; set; }
        public int TotalPageCount { get; set; }
    }
}
