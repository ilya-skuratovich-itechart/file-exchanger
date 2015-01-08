using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace FileExchange.Core.DAL.Repository
{

    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected System.Data.Entity.DbContext _entities;
        protected  IDbSet<T> _dbset;

        public GenericRepository()
        {
            
        }
        public void InitializeDbContext(System.Data.Entity.DbContext entities)
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

        public IEnumerable<T> GetPaged<TSortKey>(System.Linq.Expressions.Expression<Func<T, TSortKey>> sortExpression, int pageNumber, int pageSize, out int pageCount, string[] includes = null)
        {
            var query = _dbset.AsQueryable();
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            pageCount = query.Count();
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            query = query
                .OrderBy(sortExpression);
            query = pageNumber == 1
                ? query.Skip(0).Take(pageSize)
                : query.Skip(((pageNumber - 1)*pageSize)).Take<T>(pageSize);
            return query.AsEnumerable();
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
            foreach (var item in _dbset.Where(predicate))
                _dbset.Remove(item);
        }

        public virtual T Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual void Update(T entity)
        {
            _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}