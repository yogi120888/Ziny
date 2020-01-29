using ChemiFriend.Entity;
using ChemiFriend.Entity.JsonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.IRepository
{
    public interface ISchemeRepository : IGenericRepository<Scheme>
    {
        IQueryable<GetSchemeModel> GetSchemeList();
        IQueryable<GetSchemeModel> GetSchemeListByDeal(Int64 DealId);
        IQueryable<GetSchemeModel> GetSchemeDetailsById(Int64 schemeId);
    }
}
