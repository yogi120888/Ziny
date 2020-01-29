using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.IRepository
{
    public interface IProductCategoryRepository : IGenericRepository<ProductCategory>
    {
        ProductCategory CreateProductCategory(ProductCategory productCategory);
        bool EditProductCategory(ProductCategory productCategory);

    }
}
