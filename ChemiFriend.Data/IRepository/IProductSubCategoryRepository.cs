using ChemiFriend.Entity.JsonModel;
using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChemiFriend.Data.IRepository
{
    public interface IProductSubCategoryRepository : IGenericRepository<ProductSubCategory>
    {
        ProductSubCategory CreateProductSubCategory(ProductSubCategory productSubCategory);
        bool EditProductSubCategory(ProductSubCategory productSubCategory);
        IQueryable<GetProductSubCategoryModel> GetProductSubCategoryList();
    }
}
