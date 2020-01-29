using ChemiFriend.Data.DataContext;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Entity;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.Repository
{
    public class PaymentRepository : GenericRepository<DataBaseContext, Payment>, IPaymentRepository
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        #region [Constructor]
        public PaymentRepository()
        {
            _entities = new DataBaseContext();
        }
        #endregion

    }
}
