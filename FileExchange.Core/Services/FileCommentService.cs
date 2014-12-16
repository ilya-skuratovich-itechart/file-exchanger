using System;
using System.Linq;
using Autofac;
using EntityFramework.Extensions;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Repositories;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Services
{
    public class FileCommentService : IFileCommentService
    {
        private IGenericRepository<FileComments> _repository;

        public FileCommentService(IUnitOfWork unitOfWork)
        {
            _repository = BootStrap.Container.Resolve<IGenericRepository<FileComments>>();
            _repository.InitializeDbContext(unitOfWork.DbContext);
        }

        public FileComments Add(int fileId, string comment, DateTime createDate)
        {
            return _repository.Add(
                new FileComments()
                {
                    Comment = comment,
                    FileId = fileId,
                    CreateDate = DateTime.UtcNow
                });
        }

        public void RemoveAll(int fileId)
        {
            _repository.RemoveBy(c => c.FileId == fileId);
        }
    }
}