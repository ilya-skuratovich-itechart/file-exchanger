using System;
using System.Collections.Generic;
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

        public FileComments Add(int fileId, string comment)
        {
            return _repository.Add(
                new FileComments()
                {
                    Comment = comment,
                    FileId = fileId,
                    CreateDate = DateTime.UtcNow
                });
        }

        public IEnumerable<FileComments> GetFileComments(int fileId)
        {
           return _repository.FindBy(c => c.FileId == fileId);
        }
        public void RemoveAll(int fileId)
        {
            _repository.RemoveBy(c => c.FileId == fileId);
        }
    }
}