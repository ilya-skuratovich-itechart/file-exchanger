namespace FileExchange.Core.UOW
{
    public interface IUnitOfWork
    {
        FileExchangeDbContext DbContext { get; }
        void StartTransaction();
        void Rollback();
        void Commit();
    }
}