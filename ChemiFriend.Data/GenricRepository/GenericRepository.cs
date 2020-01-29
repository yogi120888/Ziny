using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data
{
    public abstract class GenericRepository<C, T> :
      IGenericRepository<T> where T : class where C : DbContext, IDisposable, new()
    {

        private C _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public GenericRepository()
        {
            _entities = new C();
        }
        public C Context
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            try
            {
                _entities.Set<T>().Add(entity);
            }
            catch (Exception ex)
            {
                Logger.Error("GenericRepository.Add(T entity) : " + ex.Message, ex);
            }
        }

        public virtual bool Delete(T entity)
        {
            try
            {
                _entities.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {
                Logger.Error("GenericRepository.Delete(T entity) : " + ex.Message, ex);
            }
            return false;
        }

        public virtual bool Update(T entity)
        {
            try
            {
                _entities.Entry(entity).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("GenericRepository.Edit(T entity) : " + ex.Message, ex);
                return false;
            }
        }

        public virtual int SaveChanges()
        {
            try
            {
                return _entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error("GenericRepository.Save() : " + ex.Message, ex);
            }
            return 0;
        }
        public virtual bool SaveList(List<T> entity)
        {
            try
            {
                _entities.Set<T>().AddRange(entity);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("GenericRepository.SaveList(List<T> entity) : " + ex.Message, ex);
            }
            return false;
        }

        public virtual bool Save(T entity)
        {
            try
            {
                _entities.Set<T>().Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("GenericRepository.Save(T entity) : " + ex.Message, ex);
            }
            return false;
        }
        public virtual T SaveReturnModel(T entity)
        {
            try
            {
                _entities.Set<T>().Add(entity);
                int rows = _entities.SaveChanges();
                return rows > 0 ? entity : null;
            }
            catch (Exception ex)
            {
                Logger.Error("GenericRepository.SaveReturnModel(T entity) : " + ex.Message, ex);
            }
            return null;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _entities.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
