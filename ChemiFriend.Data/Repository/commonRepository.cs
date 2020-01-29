using ChemiFriend.Data.DataContext;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Entity;
using ChemiFriend.ENTITY;
using ChemiFriend.Utility;
using System.Linq;

namespace ChemiFriend.Data.Repository
{
    public class commonRepository : IcommonRepository
    {
        private DataBaseContext _entities;
        public commonRepository()
        {
            _entities = new DataBaseContext();
        }
        public IQueryable<Country> GetCountry()
        {
            return _entities.Countries.Where(x => x.Status == true);
        }
        public IQueryable<State> GetState(int CountryId)
        {
            return _entities.States.Where(x => x.Status == true && x.CountryId == CountryId);
        }

        public IQueryable<UserRole> GetUserRoleList()
        {
            return _entities.UserRoles.Where(x => x.Status == true && x.RoleId != (int)Roles.Admin);
        }

        public IQueryable<SchemeType> GetSchemeType()
        {
            return _entities.SchemeTypes.Where(x => x.Status == true);
        }

        public IQueryable<DealType> GetDealType()
        {
            return _entities.DealTypes.Where(x => x.Status == true);
        }

        public IQueryable<FormType> GetFormType()
        {
            return _entities.FormTypes.Where(x => x.Status == true);
        }

        public IQueryable<PackType> GetPackType()
        {
            return _entities.PackTypes.Where(x => x.Status == true);
        }

        public IQueryable<ProductType> GetProductType()
        {
            return _entities.ProductTypes.Where(x => x.Status == true);
        }

        public IQueryable<GSTApplicable> GetGSTApplicable()
        {
            return _entities.GSTApplicables.Where(x => x.Status == true);
        }

        public IQueryable<DealApplicableFor> GetDealApplicableFor()
        {
            return _entities.DealApplicableFors.Where(x => x.Status == true);
        }

        public IQueryable<ProductCode> GetProductCode()
        {
            return _entities.productCodes.Where(x => x.Status == true);
        }

    }
}
