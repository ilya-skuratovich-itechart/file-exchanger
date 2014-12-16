using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;

namespace FileExchange.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void InitializeDbContext(DbContext entities);
        IEnumerable<T> GetAll(string[] includes = null);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, string[] includes = null);

        IEnumerable<T> GetPaged(int pageNumber, int pageSize, string[] includes = null);

        IEnumerable<T> FindPaged(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int pageNumber,
            int pageSize,string[] includes=null);

        void RemoveBy(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Delete(T entity);
        void Edit(T entity);
        void Save();
    }
}