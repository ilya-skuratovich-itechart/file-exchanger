using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;

namespace FileExchange.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void InitializeDbContext(DbContext entities);
        IEnumerable<T> GetAll();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Delete(T entity);
        void Edit(T entity);
        void Save();
    }
}