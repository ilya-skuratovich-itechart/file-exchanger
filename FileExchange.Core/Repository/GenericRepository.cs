﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Repositories
{

    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected DbContext _entities;
        protected  IDbSet<T> _dbset;

        public void InitializeDbContext(DbContext entities)
        {
            _entities = entities;
            _dbset = entities.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {

            return _dbset.AsEnumerable<T>();
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
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