using System;
using System.Transactions;

namespace FileExchange.Core.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        FileExchangeDbContext DbContext { get; }
        TransactionScope BeginTransaction();
        void Rollback();
        void SaveChanges();
    }
}