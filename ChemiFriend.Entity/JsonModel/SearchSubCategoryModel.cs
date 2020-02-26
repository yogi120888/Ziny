﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity.JsonModel
{
    public class SearchSubCategoryModel 
    {
        public Int64 CategoryId { get; set; }
        public string Search { get; set; }
        public List<GetProductSubCategoryModel> getSubCategoryModel { get; set; }
    }
}
