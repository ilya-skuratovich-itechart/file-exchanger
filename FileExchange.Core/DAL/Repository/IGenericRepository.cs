using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FileExchange.Core.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        void InitializeDbContext(System.Data.Entity.DbContext entities);
        IEnumerable<T> GetAll(string[] includes = null);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, string[] includes = null);

        IEnumerable<T> GetPaged<TSortKey>(System.Linq.Expressions.Expression<Func<T, TSortKey>> sortExpression, int pageNumber, int pageSize, out int pageCount, string[] includes = null);

        IEnumerable<T> FindPaged<TSortKey>(System.Linq.Expressions.Expression<Func<T, bool>> predicate,
            System.Linq.Expressions.Expression<Func<T, TSortKey>> sortExpression,
            int startDisplayRec,
            int displayLenght,
            out int totalRecords,
            string[] includes = null);

        void RemoveBy(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Delete(T entity);
        void Update(T entity);
        void Save();
    }
}