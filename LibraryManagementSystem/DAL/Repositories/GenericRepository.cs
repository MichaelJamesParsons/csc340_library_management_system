using System;
using System.Data.Entity;
using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.DAL.Repositories
{
    public abstract class GenericRepository<C, T> :
        IGenericRepository<T> where T : class, IModelKey where C : DbContext, new()
    {
        private C _entities = new C();

        protected C Context
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public virtual IQueryable<T> GetAll()
        {
            return _entities.Set<T>();
        }
        
        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _entities.Set<T>().Where(predicate);
        }

        public virtual T Find(int id)
        {
            return _entities.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            try
            {
                _entities.SaveChanges();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                throw e;
            }
        }

        public virtual void ReloadRepository(T entity)
        {
            Context.Entry(entity).GetDatabaseValues();
        }
    }
}