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
    public class SchemeRepository : GenericRepository<DataBaseContext, Scheme>, ISchemeRepository
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        #region [Constructor]
        public SchemeRepository()
        {
            _entities = new DataBaseContext();
        }
        #endregion

        #region[Public Mehods]

        public IQueryable<GetSchemeModel> GetSchemeList()
        {
            var query = (from sc in _entities.Schemes
                         join st in _entities.SchemeTypes on sc.SchemeTypeId equals st.SchemeTypeId
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
                             ModifiedDate = sc.ModifiedDate
                         });
            return query;
        }
        public IQueryable<GetSchemeModel> GetSchemeListByDeal(Int64 DealId)
        {
            var query = (from sc in _entities.Schemes
                         join st in _entities.SchemeTypes on sc.SchemeTypeId equals st.SchemeTypeId
                         where sc.DealId == DealId
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
                             ModifiedDate = sc.ModifiedDate
                         });
            return query;
        }

        public IQueryable<GetSchemeModel> GetSchemeDetailsById(Int64 schemeId)
        {
            var query = (from sc in _entities.Schemes
                         join st in _entities.SchemeTypes on sc.SchemeTypeId equals st.SchemeTypeId
                         where sc.SchemeId == schemeId && sc.Status == (int)Status.Active && sc.IsDeleted == false
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
                             ModifiedDate = sc.ModifiedDate
                         });
            return query;
        }

        #endregion

    }
}
