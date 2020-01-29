using ChemiFriend.Entity.JsonModel;
using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.IRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Product CreateProduct(Product product);
        bool EditProduct(Product product);
        IQueryable<GetProductModel> GetProductList();
        IQueryable<GetProductModel> BindProducts();
    }
}
