using System;
using System.Transactions;
using FileExchange.Core.DAL;
using FileExchange.Core.DAL.DbContext;

namespace FileExchange.Core.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        FileExchangeDbContext DbContext { get; }
        void BeginTransaction();
        void CommitTransaction();
        void Rollback();
        void SaveChanges();
    }
}