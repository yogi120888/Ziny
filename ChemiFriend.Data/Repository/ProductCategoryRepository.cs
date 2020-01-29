using ChemiFriend.Data.DataContext;
using ChemiFriend.Data.IRepository;
using ChemiFriend.ENTITY;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.Repository
{
    public class ProductCategoryRepository : GenericRepository<DataBaseContext, ProductCategory>, IProductCategoryRepository
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        #region [Constructor]
        public ProductCategoryRepository()
        {
            _entities = new DataBaseContext();
        }
        #endregion

        #region[Public Method]

        public ProductCategory CreateProductCategory(ProductCategory productCategory)
        {
            try
            {
                base.Add(productCategory);
                int rows = base.SaveChanges();
                return rows > 0 ? productCategory : null;
            }
            catch (Exception ex)
            {
                Logger.Error("ProductCategoryRepository.CreateProductCategory(ProductCategory productCategory) : " + ex.Message, ex);
            }
            return null;
        }

        public bool EditProductCategory(ProductCategory productCategory)
        {
            try
            {
                base.Update(productCategory);
                int rows = base.SaveChanges();
                return rows > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.Error("ProductCategoryRepository.EditProductCategory(ProductCategory productCategory) : " + ex.Message, ex);
            }
            return false;
        }

        #endregion
    }
}
