using System;
using System.Linq;
using System.Linq.Expressions;

namespace LibraryManagementSystem.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Find(int id);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();
        void ReloadRepository(T entity);
    }
}
