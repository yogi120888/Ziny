using ChemiFriend.Data.DataContext;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Entity;
using ChemiFriend.Entity.JsonModel;
using ChemiFriend.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.Repository
{
    public class DealRepository : GenericRepository<DataBaseContext, Deal>, IDealRepository
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        #region [Constructor]
        public DealRepository()
        {
            _entities = new DataBaseContext();
        }
        #endregion

        #region[Public Mehods]

        public IQueryable<GetDealModel> GetDealList()
        {
            var query = (from de in _entities.Deals
                         join pr in _entities.Products on de.ProductId equals pr.ProductId
                         join ps in _entities.ProductSubCategories on pr.ProductSubCategoryId equals ps.ProductSubCategoryId
                         join pc in _entities.ProductCategories on pr.ProductCategoryId equals pc.ProductCategoryId
                         join pdc in _entities.productCodes on pr.ProductCodeId equals pdc.ProductCodeId
                         join pts in _entities.ProductTypes on de.ProductTypeId equals pts.ProductTypeId
                         join dt in _entities.DealTypes on de.DealType equals dt.DealTypeId
                         join fo in _entities.FormTypes on de.FormType equals fo.FormTypeId
                         join pt in _entities.PackTypes on de.PackType equals pt.PackTypeId
                         join ga in _entities.GSTApplicables on de.GST equals ga.GSTApplicableId
                         join daf in _entities.DealApplicableFors on de.DealApplicableFor equals daf.DealApplicableForId
                         join um in _entities.Usermasters on de.CreatedBy equals um.UserId
                         where de.IsDeleted == false
                         select new GetDealModel
                         {
                             DealId = de.DealId,
                             DealType = de.DealType,
                             DealTypeName = dt.DealTypeName,
                             DealApplicableForId = de.DealApplicableFor,
                             DealApplicableFor = daf.Applicable,
                             ProductCategory = pc.ProductCategoryName,
                             ProductSubCategoryId = pr.ProductSubCategoryId.Value,
                             ProductSubCategory = ps.ProductSubCategoryName,
                             ProductImagePath = de.ProductImagePath,
                             ProductId = de.ProductId,
                             ProductName = pr.ProductName,
                             ProductCodeId = pr.ProductCodeId,
                             ProductCode = pdc.ProductCodes,
                             ProductTypeId = de.ProductTypeId,
                             DealStartDate = de.DealStartDate,
                             DealEndDate = de.DealEndDate,
                             ProductExpiryDate = de.ProductExpiryDate,
                             MarketingCompany = de.MarketingCompany,
                             Brand = de.Brand,
                             FormTypeId = de.FormType,
                             FormType = fo.FormTypeName,
                             ProductType = pts.ProductTypeName,
                             PackType = pt.PackTypeName,
                             MRP = de.MRP,
                             PTR = de.PTR,
                             Composition = de.Composition,
                             StockAvailable = de.StockAvailable == null ? 0 : de.StockAvailable.Value,
                             MaxQuantityForRetailer = de.MaxQuantityForRetailer == null ? 0 : de.MaxQuantityForRetailer.Value,
                             PackTypeId = de.PackType,
                             PackSize = de.PackSize,
                             GSTApplicableId = de.GST,
                             GST = ga.GST,
                             Status = de.Status,
                             IsDeleted = de.IsDeleted,
                             CreatedBy = de.CreatedBy,
                             CreatedDate = de.CreatedDate,
                             ModifiedBy = de.ModifiedBy,
                             ModifiedDate = de.ModifiedDate,
                             DealEndDays = de.DealEndDate.Day - de.DealStartDate.Day,
                             DealEndHours = de.DealEndDate.Hour - de.DealStartDate.Hour,
                             User = um.Name + "(" + um.Phone + ")"
                         });
            return query;
        }

        public IQueryable<GetSchemeModel> GetSchemeList()
        {
            var query = (from sc in _entities.Schemes
                         join st in _entities.SchemeTypes on sc.SchemeTypeId equals st.SchemeTypeId
                         join um in _entities.Usermasters on sc.CreatedBy equals um.UserId
                         where sc.IsDeleted == false && sc.Status == (int)Status.Active
                         select new GetSchemeModel
                         {
                             SchemeId = sc.SchemeId,
                             DealId = sc.DealId,
                             SchemeTypeId = sc.SchemeTypeId,
                             SchemeType = st.SchemeTypeName,
                             MinOrderQuantity = sc.MinOrderQuantity,
                             Discount = sc.Discount,
                             DealRate = sc.DealRate,
                             Saving = sc.Saving,
                             DealScheme = sc.DealScheme,
                             Status = sc.Status,
                             IsDeleted = sc.IsDeleted,
                             CreatedBy = sc.CreatedBy,
                             CreatedDate = sc.CreatedDate,
                             ModifiedBy = sc.ModifiedBy,
                             ModifiedDate = sc.ModifiedDate,
                             User = um.Name + "(" + um.Phone + ")"
                         });
            return query;
        }

        public IQueryable<GetDealModel> GetDealDetailsById(Int64 dealId)
        {
            var query = (from de in _entities.Deals
                         join dt in _entities.DealTypes on de.DealType equals dt.DealTypeId
                         join fo in _entities.FormTypes on de.FormType equals fo.FormTypeId
                         join pt in _entities.PackTypes on de.PackType equals pt.PackTypeId
                         join ga in _entities.GSTApplicables on de.GST equals ga.GSTApplicableId
                         where de.DealId == dealId && de.Status == (int)Status.Active && de.IsDeleted == false
                         select new GetDealModel
                         {
                             DealId = de.DealId,
                             DealType = de.DealType,
                             DealTypeName = dt.DealTypeName,
                             DealApplicableForId = de.DealApplicableFor,
                             ProductId = de.ProductId,
                             ProductName = de.ProductName,
                             ProductTypeId = de.ProductTypeId,
                             DealStartDate = de.DealStartDate,
                             DealEndDate = de.DealEndDate,
                             ProductExpiryDate = de.ProductExpiryDate,
                             MarketingCompany = de.MarketingCompany,
                             Brand = de.Brand,
                             FormTypeId = de.FormType,
                             FormType = fo.FormTypeName,
                             MRP = de.MRP,
                             PTR = de.PTR,
                             Composition = de.Composition,
                             StockAvailable = de.StockAvailable == null ? 0 : de.StockAvailable.Value,
                             MaxQuantityForRetailer = de.MaxQuantityForRetailer == null ? 0 : de.MaxQuantityForRetailer.Value,
                             PackTypeId = de.PackType,
                             PackType = pt.PackTypeName,
                             PackSize = de.PackSize,
                             GSTApplicableId = de.GST,
                             GST = ga.GST,
                             Status = de.Status,
                             IsDeleted = de.IsDeleted,
                             CreatedBy = de.CreatedBy,
                             CreatedDate = de.CreatedDate,
                             ModifiedBy = de.ModifiedBy,
                             ModifiedDate = de.ModifiedDate
                         });

            return query;
        }

        public IQueryable<GetSchemeWithDealModel> GetSchemeWithDeal()
        {
            var query = (from sc in _entities.Schemes
                         join de in _entities.Deals on sc.DealId equals de.DealId
                         join pr in _entities.Products on de.ProductId equals pr.ProductId
                         join pc in _entities.ProductCategories on pr.ProductCategoryId equals pc.ProductCategoryId
                         join st in _entities.SchemeTypes on sc.SchemeTypeId equals st.SchemeTypeId
                         join pt in _entities.ProductTypes on de.ProductTypeId equals pt.ProductTypeId
                         join ft in _entities.FormTypes on de.FormType equals ft.FormTypeId
                         join pk in _entities.PackTypes on de.PackType equals pk.PackTypeId
                         where sc.Status == (int)Status.Active && sc.IsDeleted == false
                         select new GetSchemeWithDealModel
                         {
                             SchemeId = sc.SchemeId,
                             DealId = sc.DealId,
                             SchemeType = st.SchemeTypeName,
                             MinOrderQuantity = sc.MinOrderQuantity,
                             Discount = sc.Discount,
                             DealRate = sc.DealRate,
                             Saving = sc.Saving,
                             DealScheme = sc.DealScheme,
                             ProductCategory = pc.ProductCategoryName,
                             ProductId = de.ProductId, // to do
                             ProductName = pr.ProductName,
                             ProductImagePath = de.ProductImagePath, // to do
                             ProductType = pt.ProductTypeName, // to do
                             DealStartDate = de.DealStartDate,
                             DealEndDate = de.DealEndDate,
                             ProductExpiryDate = de.ProductExpiryDate,
                             MarketingCompany = de.MarketingCompany,
                             Brand = de.Brand,
                             FormType = ft.FormTypeName,
                             MRP = de.MRP,
                             PTR = de.PTR,
                             Composition = de.Composition,
                             StockAvailable = de.StockAvailable == null ? 0 : de.StockAvailable.Value,
                             PackType = pk.PackTypeName,
                             PackSize = de.PackSize.ToString(),
                             Status = sc.Status,
                             IsDeleted = sc.IsDeleted,
                             CreatedBy = sc.CreatedBy,
                             CreatedDate = sc.CreatedDate,
                             ModifiedBy = sc.ModifiedBy,
                             ModifiedDate = sc.ModifiedDate
                         });

            return query;
        }

        public IQueryable<OrderDetailModel> GetOrderDetail()
        {
            var query = (from pr in Context.Products
                         join dl in Context.Deals on pr.ProductId equals dl.ProductId
                         join sc in Context.Schemes on dl.DealId equals sc.DealId
                         select new OrderDetailModel
                         {
                             DealId = dl.DealId,
                             ProductId = pr.ProductId,
                             Product = pr.ProductName,
                             ProductImagePath = pr.ProductImagePath,
                             SchemeId = sc.SchemeId,
                             Scheme = sc.DealScheme,
                             DealPrice = dl.PTR,
                         });
            return query;
        }


        #endregion
    }
}
