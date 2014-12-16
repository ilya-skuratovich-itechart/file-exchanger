using Autofac;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Repositories;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Services
{
    public class FileNotificationSubscriberService : IFileNotificationSubscriberService
    {
         private IGenericRepository<FileNotificationSubscribers> _repository;

         public FileNotificationSubscriberService(IUnitOfWork unitOfWork)
        {
            _repository = BootStrap.Container.Resolve<IGenericRepository<FileNotificationSubscribers>>();
            _repository.InitializeDbContext(unitOfWork.DbContext);
        }

        public FileNotificationSubscribers Add(int userId, int fileId)
        {
            return _repository.Add(
                new FileNotificationSubscribers()
                {
                    UserId = userId,
                    FileId = fileId

                });
        }

        public void RemoveAll(int fileId)
        {
           _repository.RemoveBy(s=>s.FileId==fileId);
        }

        public void RemoveFromUser(int fileId, int userId)
        {
             _repository.RemoveBy(s=>s.FileId==fileId && s.UserId==userId);
        }
    }
}