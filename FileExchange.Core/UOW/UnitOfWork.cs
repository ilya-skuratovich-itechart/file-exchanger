using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Transactions;

namespace FileExchange.Core.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private FileExchangeDbContext _fileExchangeContext { get; set; }
        public FileExchangeDbContext DbContext
        {
            get { return _fileExchangeContext; }
        }

        public UnitOfWork()
        {
            _fileExchangeContext = new FileExchangeDbContext();
        }

        public TransactionScope BeginTransaction()
        {
           return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted });
        }

        public void SaveChanges()
        {
            _fileExchangeContext.SaveChanges();
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
            if (_fileExchangeContext != null)
            {
                _fileExchangeContext.Dispose();
                _fileExchangeContext = null;
            }
        }

    }
}