using System;
using System.Transactions;

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