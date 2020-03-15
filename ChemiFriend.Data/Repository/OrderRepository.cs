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
    public class OrderRepository : GenericRepository<DataBaseContext, Order>, IOrderRepository
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        #region [Constructor]
        public OrderRepository()
        {
            _entities = new DataBaseContext();
        }
        #endregion

        public IQueryable<GetCustomerOrderModel> GetOrderList()
        {
            var query = (from od in _entities.Orders
                         join rg in _entities.Registrations on od.UserId equals rg.UserId
                         join st in _entities.States on rg.State equals st.StateId
                         orderby od.CreatedDate descending
                         select new GetCustomerOrderModel
                         {
                             OrderId = od.OrderId,
                             UserId = od.UserId,
                             Retailer = rg.FirstName + " " + rg.LastName,
                             RetailerPhone = rg.PhoneNo,
                             RetailerCity = rg.City,
                             RetailerState = st.StateName,
                             ZipCode = rg.ZipCode,
                             RetailerAddress = rg.Address1,
                             GrandTotal = od.GrandTotal,
                             PaymentStatus = od.PaymentStatus,
                             Status = od.Status,
                             IsDeleted = od.IsDeleted,
                             CreatedBy = od.CreatedBy,
                             CreatedDate = od.CreatedDate,
                             ModifiedBy = od.ModifiedBy,
                             ModifiedDate = od.ModifiedDate,
                         });

            return query;
        }


    }
}
