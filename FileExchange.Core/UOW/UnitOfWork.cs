using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Transactions;

namespace FileExchange.Core.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private FileExchangeDbContext _fileExchangeContext { get; set; }
        private TransactionScope _transaction;
        public FileExchangeDbContext DbContext
        {
            get { return _fileExchangeContext; }
        }

        public UnitOfWork(FileExchangeDbContext fileExchangeContext)
        {
            _fileExchangeContext = fileExchangeContext;
        }

        public void StartTransaction()
        {
            _transaction = new TransactionScope();
        }

        public void Commit()
        {
            _fileExchangeContext.SaveChanges();
            if (_transaction != null)
            {
                _transaction.Complete();
            }
        }

        public void Rollback()
        {
            var context = ((IObjectContextAdapter)_fileExchangeContext).ObjectContext;
            foreach (var change in _fileExchangeContext.ChangeTracker.Entries())
            {
                if (change.State == EntityState.Modified || change.State == EntityState.Deleted)
                {
                    context.Refresh(RefreshMode.StoreWins, change.Entity);
                }
                if (change.State == EntityState.Added)
                {
                    context.Detach(change.Entity);
                }
            }
        }

        public void Dispose()
        {
            _fileExchangeContext.Dispose();
            _fileExchangeContext = null;
        }
       
    }
}