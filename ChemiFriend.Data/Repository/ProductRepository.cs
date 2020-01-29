using ChemiFriend.Data.DataContext;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Entity.JsonModel;
using ChemiFriend.ENTITY;
using ChemiFriend.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ChemiFriend.Data.Repository
{
    public class ProductRepository : GenericRepository<DataBaseContext, Product>, IProductRepository
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        #region [Constructor]
        public ProductRepository()
        {
            _entities = new DataBaseContext();
        }
        #endregion

        #region[Public Method]

        public Product CreateProduct(Product product)
        {
            try
            {
                base.Add(product);
                int rows = base.SaveChanges();
                return rows > 0 ? product : null;
            }
            catch (Exception ex)
            {
                Logger.Error("ProductRepository.CreateProduct(Product Product) : " + ex.Message, ex);
            }
            return null;
        }

        public bool EditProduct(Product Product)
        {
            try
            {
                base.Update(Product);
                int rows = base.SaveChanges();
                return rows > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.Error("ProductRepository.EditProduct(Product Product) : " + ex.Message, ex);
            }
            return false;
        }

        public IQueryable<GetProductModel> GetProductList()
        {
            var query = (from pd in _entities.Products
                         join pc in _entities.ProductCategories on pd.ProductCategoryId equals pc.ProductCategoryId
                         join psc in _entities.ProductSubCategories on pd.ProductSubCategoryId equals psc.ProductSubCategoryId
                         join pt in _entities.ProductTypes on pd.ProductTypeId equals pt.ProductTypeId
                         select new GetProductModel
                         {
                             ProductId = pd.ProductId,
                             ProductCategoryId = pd.ProductCategoryId,
                             ProductCategory = pc.ProductCategoryName,
                             ProductSubCategoryId = pd.ProductSubCategoryId,
                             ProductSubCategory = psc.ProductSubCategoryName,
                             ProductName = pd.ProductName,
                             ProductImagePath = pd.ProductImagePath,
                             ProductTypeId = pd.ProductTypeId,
                             ProductType = pt.ProductTypeName,
                             MarketingCompany = pd.MarketingCompany,
                             MRP = pd.MRP,
                             PTR = pd.PTR,
                             //Composition = pd.Composition,
                             //StockAvailable = pd.StockAvailable,
                             Status = pd.Status,
                             IsDeleted = pd.IsDeleted,
                             CreatedBy = pd.CreatedBy,
                             CreatedDate = pd.CreatedDate,
                             ModifiedBy = pd.ModifiedBy,
                             ModifiedDate = pd.ModifiedDate
                         });

            return query;
        }

        public IQueryable<GetProductModel> BindProducts()
        {
            var query = (from pd in _entities.Products
                         join pc in _entities.ProductCategories on pd.ProductCategoryId equals pc.ProductCategoryId
                         join psc in _entities.ProductSubCategories on pd.ProductSubCategoryId equals psc.ProductSubCategoryId
                         where pd.Status == (int)Status.Active
                         select new GetProductModel
                         {
                             ProductId = pd.ProductId,
                            // ProductCategory = pc.ProductCategoryName,
                             ProductSubCategory = pc.ProductCategoryName + " => " + psc.ProductSubCategoryName,
                             ProductName = pc.ProductCategoryName + " => " + psc.ProductSubCategoryName + " => " + pd.ProductName,
                         });
            return query;
        }

        #endregion
    }
}
