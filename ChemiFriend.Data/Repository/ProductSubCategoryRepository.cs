using ChemiFriend.Data.DataContext;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Entity.JsonModel;
using ChemiFriend.ENTITY;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.Repository
{
    public class ProductSubCategoryRepository : GenericRepository<DataBaseContext, ProductSubCategory>, IProductSubCategoryRepository
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        #region [Constructor]
        public ProductSubCategoryRepository()
        {
            _entities = new DataBaseContext();
        }
        #endregion

        #region[Public Method]

        public ProductSubCategory CreateProductSubCategory(ProductSubCategory productSubCategory)
        {
            try
            {
                base.Add(productSubCategory);
                int rows = base.SaveChanges();
                return rows > 0 ? productSubCategory : null;
            }
            catch (Exception ex)
            {
                Logger.Error("ProductSubCategoryRepository.CreateProductSubCategory(ProductSubCategory productSubCategory) : " + ex.Message, ex);
            }
            return null;
        }

        public bool EditProductSubCategory(ProductSubCategory productSubCategory)
        {
            try
            {
                base.Update(productSubCategory);
                int rows = base.SaveChanges();
                return rows > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.Error("ProductSubCategoryRepository.EditProductSubCategory(ProductSubCategory productSubCategory) : " + ex.Message, ex);
            }
            return false;
        }

        public IQueryable<GetProductSubCategoryModel> GetProductSubCategoryList()
        {
            var query = (from psc in _entities.ProductSubCategories
                         join pc in _entities.ProductCategories on psc.ProductCategoryId equals pc.ProductCategoryId
                         select new GetProductSubCategoryModel
                         {
                             ProductSubCategoryId = psc.ProductSubCategoryId,
                             ProductCategoryId = psc.ProductCategoryId,
                             ProductCategory = pc.ProductCategoryName,
                             ProductSubCategoryName = psc.ProductSubCategoryName,
                             ProductSubCategoryImagePath = pc.ProductCategoryImagePath,
                             Status = psc.Status,
                             IsDeleted = psc.IsDeleted,
                             CreatedBy = psc.CreatedBy,
                             CreatedDate = psc.CreatedDate,
                             ModifiedBy = psc.ModifiedBy,
                             ModifiedDate = psc.ModifiedDate
                         });

            return query;
        }

        #endregion

    }

}
