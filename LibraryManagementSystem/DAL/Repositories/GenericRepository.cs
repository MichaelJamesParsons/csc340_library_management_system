using System;
using System.Data.Entity;
using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.DAL.Repositories
{
    /// <summary>
    ///     The GenericRepository object acts as a middle man between the database and the application. It is in charge of managing
    ///     objects that have been cached in memory and syncing the cache with the database records.
    /// </summary>
    /// <typeparam name="TC">A database contentext. It must extend DbContext.</typeparam>
    /// <typeparam name="T">
    ///     The type of object that will be stored in the repository. This object type must
    ///     use the IModelKey interface.
    /// </typeparam>
    public abstract class GenericRepository<TC, T> :
        IGenericRepository<T> where T : class, IModelKey where TC : DbContext, IDisposable, new()
    {
        //Indicates whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).
        //See GenericRepository:Dispose()
        private bool _disposed;

        //The database context for the type of object being stored in the repository
        protected TC Context { get; set; } = new TC();


        /// <summary>
        /// Fetches all of the object records.
        /// </summary>
        /// <returns>A list of T objects</returns>
        public virtual IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }


        /// <summary>
        /// Fetches objects that meet certain conditions. This method will first check the cache for the
        /// matching records. If it doesn't exist, it will then check the database.
        /// </summary>
        /// <param name="predicate">A set of query conditions in the form of a lambda expression.</param>
        /// <returns>IQueryable result set</returns>
        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }


        /// <summary>
        /// Fetches a single object with a specific Id. This method will first check the cache for the
        /// record. If it doesn't exist, it will then check the database.
        /// </summary>
        /// <param name="id">The Id of the object's record.</param>
        /// <returns>T object</returns>
        public virtual T Find(int? id)
        {
            return (id == null) ? null : Context.Set<T>().FirstOrDefault(x => x.Id == id);
        }


        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }


        /// <summary>
        /// Deletes entity from the repository.
        /// </summary>
        /// <param name="entity">The entity in the form of an object)].</param>
        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }


        /// <summary>
        /// Updates an entit's record in the repository.
        /// </summary>
        /// <param name="entity">The entity in the form of an object.</param>
        public virtual void Edit(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }


        /// <summary>
        /// Syncs the repository with the database. This is allows many operations to be
        /// performed on the repository before applying the changes to the database records
        /// in the form of a transaction.
        /// </summary>
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


        /// <summary>
        /// Flushes the cache for a specific entity and resyncs the entity's
        /// cache record with the database.
        /// </summary>
        /// <param name="entity">The entity in the form of an object.</param>
        public virtual void ReloadRepository(T entity)
        {
            Context.Entry(entity).GetDatabaseValues();
        }


        /// <summary>
        /// Releases loose objects and other unmanaged resources when the controller
        /// is no longer in use.
        /// Documentation: https://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx
        /// </summary>
        /// <param name="disposing">Indicates whether the method call comes from a 
        ///             Dispose method (its value is true) or from a finalizer (its value is false).
        /// </param>
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }


        /// Override
        /// 
        /// <summary>
        /// Releases loose objects and other unmanaged resources when the controller
        /// is no longer in use.
        /// Documentation: https://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}