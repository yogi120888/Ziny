using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data
{
    public interface IGenericRepository<T> where T : class 
    {
        IQueryable<T> GetAll();       
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        bool Delete(T entity);
        bool Update(T entity);
        int SaveChanges();
        bool SaveList(List<T> entity);
        bool Save(T entity);
        T SaveReturnModel(T entity);
        void Dispose();
    }
}
