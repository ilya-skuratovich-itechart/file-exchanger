using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using EntityFramework.Extensions;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Repositories
{

    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected DbContext _entities;
        protected  IDbSet<T> _dbset;

        public GenericRepository()
        {
            
        }
        public void InitializeDbContext(DbContext entities)
        {
            _entities = entities;
            _dbset = entities.Set<T>();
        }

        public virtual IEnumerable<T> GetAll(string[] includes = null)
        {
            var query = _dbset.AsQueryable();
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return query.AsEnumerable<T>();
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate,
                                    string[] includes = null)
        {

            var query = _dbset.AsQueryable();
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return query.Where(predicate).AsEnumerable<T>();
        }

        public IEnumerable<T> GetPaged(int pageNumber, int pageSize, string[] includes = null)
        {
            var query = _dbset.AsQueryable();
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return query
                .AsEnumerable()
               .Skip(pageNumber-1*pageSize)
               .Take(pageSize);
        }

        public IEnumerable<T> FindPaged<TSortKey>(System.Linq.Expressions.Expression<Func<T, bool>> predicate,
            System.Linq.Expressions.Expression<Func<T, TSortKey>> sortExpression,
            int startDisplayRec,
            int displayLenght,
            out int totalRecords,
            string[] includes = null)
        {
            var query = _dbset.AsQueryable();
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            query = query.OrderBy(sortExpression);
            totalRecords = query.Count();
            return query
                .Where(predicate)
                .Skip(startDisplayRec)
                .Take(displayLenght)
                .AsEnumerable();
        }

        public void RemoveBy(Expression<Func<T, bool>> predicate)
        {
            _dbset.Where(predicate).Delete();
        }

        public virtual T Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}