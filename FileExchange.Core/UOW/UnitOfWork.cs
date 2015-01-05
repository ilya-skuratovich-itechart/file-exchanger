using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Text;
using System.Transactions;

namespace FileExchange.Core.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private FileExchangeDbContext _fileExchangeContext { get; set; }
        private TransactionScope _transactionScope { get; set; }
        public FileExchangeDbContext DbContext
        {
            get { return _fileExchangeContext; }
        }

        public UnitOfWork()
        {
            _fileExchangeContext = new FileExchangeDbContext();
        }

        public void BeginTransaction()
        {
           _transactionScope= new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted });
        }

        public void CommitTransaction()
        {
            _transactionScope.Complete();
        }

        public void SaveChanges()
        {
            try
            {
                _fileExchangeContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                );
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
            if (_fileExchangeContext != null)
            {
                _fileExchangeContext.Dispose();
                _fileExchangeContext = null;
            }
            if (_transactionScope!=null)
                _transactionScope.Dispose();
        }

    }
}