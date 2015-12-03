using System;
using System.Linq;
using System.Linq.Expressions;

namespace LibraryManagementSystem.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Fetches all of the object records.
        /// </summary>
        /// <returns>A list of T objects</returns>
        IQueryable<T> GetAll();


        /// <summary>
        /// Fetches objects that meet certain conditions. This method will first check the cache for the
        /// matching records. If it doesn't exist, it will then check the database.
        /// </summary>
        /// <param name="predicate">A set of query conditions in the form of a lambda expression.</param>
        /// <returns>IQueryable result set</returns>
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);


        /// <summary>
        /// Fetches a single object with a specific Id. This method will first check the cache for the
        /// record. If it doesn't exist, it will then check the database.
        /// </summary>
        /// <param name="id">The Id of the object's record.</param>
        /// <returns>T object</returns>
        T Find(int? id);


        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(T entity);


        /// <summary>
        /// Deletes entity from the repository.
        /// </summary>
        /// <param name="entity">The entity in the form of an object)].</param>
        void Delete(T entity);


        /// <summary>
        /// Updates an entit's record in the repository.
        /// </summary>
        /// <param name="entity">The entity in the form of an object.</param>
        void Edit(T entity);


        /// <summary>
        /// Syncs the repository with the database. This is allows many operations to be
        /// performed on the repository before applying the changes to the database records
        /// in the form of a transaction.
        /// </summary>
        void Save();


        /// <summary>
        /// Flushes the cache for a specific entity and resyncs the entity's
        /// cache record with the database.
        /// </summary>
        /// <param name="entity">The entity in the form of an object.</param>
        void ReloadRepository(T entity);


        /// <summary>
        /// Releases loose objects and other unmanaged resources when the controller
        /// is no longer in use.
        /// Documentation: https://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx
        /// </summary>
        /// <param name="disposing">Indicates whether the method call comes from a 
        ///             Dispose method (its value is true) or from a finalizer (its value is false).
        /// </param>
        void Dispose(bool disposing);


        /// Override
        /// 
        /// <summary>
        /// Releases loose objects and other unmanaged resources when the controller
        /// is no longer in use.
        /// Documentation: https://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx
        /// </summary>
        void Dispose();
    }
}
