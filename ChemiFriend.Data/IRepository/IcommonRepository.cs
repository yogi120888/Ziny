using ChemiFriend.Entity;
using ChemiFriend.ENTITY;
using System.Linq;

namespace ChemiFriend.Data.IRepository
{
    public interface IcommonRepository
    {
        IQueryable<Country> GetCountry();
        IQueryable<State> GetState(int CountryId);
        IQueryable<UserRole> GetUserRoleList();
        IQueryable<SchemeType> GetSchemeType();
        IQueryable<DealType> GetDealType();
        IQueryable<FormType> GetFormType();
        IQueryable<PackType> GetPackType();
        IQueryable<ProductType> GetProductType();
        IQueryable<GSTApplicable> GetGSTApplicable();
        IQueryable<DealApplicableFor> GetDealApplicableFor();
        IQueryable<ProductCode> GetProductCode();
    }
}
