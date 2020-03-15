using ChemiFriend.Data.DataContext;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Entity;
using ChemiFriend.Entity.JsonModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.Repository
{
    public class OrderItemRepository : GenericRepository<DataBaseContext, OrderItem>, IOrderItemRepository
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        #region [Constructor]
        public OrderItemRepository()
        {
            _entities = new DataBaseContext();
        }
        #endregion

        /// <summary>
        /// Get Order Item List
        /// </summary>
        /// <returns></returns>
        public IQueryable<GetOrderItemModel> GetOrderItemList()
        {
            var query = (from oi in _entities.OrderItems
                         join od in _entities.Orders on oi.OrderId equals od.OrderId
                         join sc in _entities.Schemes on oi.SchemeId equals sc.SchemeId
                         join rg in _entities.Registrations on od.UserId equals rg.UserId
                         join pr in _entities.Products on oi.ProductId equals pr.ProductId
                         join ps in _entities.ProductSubCategories on pr.ProductSubCategoryId equals ps.ProductSubCategoryId
                         join pc in _entities.ProductCategories on ps.ProductCategoryId equals pc.ProductCategoryId
                         orderby oi.CreatedDate descending
                         select new GetOrderItemModel
                         {
                             OrderItemId = oi.OrderItemId,
                             OrderId = oi.OrderId,
                             UserId = od.UserId,
                             Retailer = rg.FirstName + " " + rg.LastName,
                             DealId = oi.DealId,
                             ProductId = oi.ProductId,
                             Product = pr.ProductName,
                             ProductSubCategoryId = pr.ProductSubCategoryId.Value,
                             Brand = ps.ProductSubCategoryName, // Brand
                             ProductCategoryId = ps.ProductCategoryId,
                             Company = pc.ProductCategoryName,
                             SchemeId = oi.SchemeId,
                             Scheme = sc.DealScheme,
                             OrderItemQuantity = oi.OrderItemQuantity,
                             OrderItemUnitPrice = oi.OrderItemUnitPrice,
                             OrderItemSchemes = oi.OrderItemSchemes,
                             Status = oi.Status,
                             IsDeleted = oi.IsDeleted,
                             CreatedBy = oi.CreatedBy,
                             CreatedDate = oi.CreatedDate,
                             ModifiedBy = oi.ModifiedBy,
                             ModifiedDate = oi.ModifiedDate,
                         });

            return query;
        }


    }
}
