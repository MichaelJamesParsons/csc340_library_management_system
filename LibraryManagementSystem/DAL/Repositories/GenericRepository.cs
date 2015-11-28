using System;
using System.Data.Entity;
using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.DAL.Repositories
{
    public abstract class GenericRepository<TC, T> :
        IGenericRepository<T> where T : class, IModelKey where TC : DbContext, IDisposable, new()
    {
        private bool _disposed;
        protected TC Context { get; set; } = new TC();

        public virtual IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }
        
        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        public virtual T Find(int? id)
        {
            return (id == null) ? null : Context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                throw;
            }
        }

        public virtual void ReloadRepository(T entity)
        {
            Context.Entry(entity).GetDatabaseValues();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}