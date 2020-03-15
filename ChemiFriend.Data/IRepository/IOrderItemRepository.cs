using ChemiFriend.Entity;
using ChemiFriend.Entity.JsonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.IRepository
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        IQueryable<GetOrderItemModel> GetOrderItemList();
    }
}
