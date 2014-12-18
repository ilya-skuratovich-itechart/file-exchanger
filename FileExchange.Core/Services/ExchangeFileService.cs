using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Repositories;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Services
{
    public class ExchangeFileService : IExchangeFileService
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<ExchangeFile> _exchangeFileRepository;

        public ExchangeFileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _exchangeFileRepository = BootStrap.Container.Resolve<IGenericRepository<ExchangeFile>>();
            _exchangeFileRepository.InitializeDbContext(unitOfWork.DbContext);
        }

        public List<ExchangeFile> GetUserFiles(int userId)
        {
            return _exchangeFileRepository
                .FindBy(val => val.UserId == userId)
                .ToList();
        }

        public List<ExchangeFile> GetAll()
        {
            return _exchangeFileRepository
                .GetAll()
                .ToList();
        }

        public ExchangeFile GetUserFile(int fileId, int userId)
        {
            return _exchangeFileRepository
              .FindBy(f => f.UserId == userId && f.FileId == fileId)
              .SingleOrDefault();
        }
        public List<ExchangeFile> GetUserFilesPaged(int userId, int startRecNum, int pageLenght, out int totalRecords)
        {
            return _exchangeFileRepository
              .FindPaged(f => f.UserId == userId, s => s.FileId, startRecNum, pageLenght, out totalRecords)
              .ToList();
        }


        public List<ExchangeFile> GetCategoryFiles(int fileCategoryId)
        {
            return _exchangeFileRepository
                .FindBy(f => f.FileCategoryId == fileCategoryId)
                .ToList();
        }

        public ExchangeFile Add(int userId, int fileCategoryId, string description, string uniqFileName, string origFileName, string tags, bool accessDenied, bool allowViewAnonymousUsers)
        {
            return _exchangeFileRepository.Add(
                new ExchangeFile()
                {
                    UserId = userId,
                    FileCategoryId = fileCategoryId,
                    Description = description,
                    UniqFileName = uniqFileName,
                    OrigFileName = origFileName,
                    Tags = tags,
                    AccessDenied = accessDenied,
                    AllowViewAnonymousUsers = allowViewAnonymousUsers,
                    CreateDate = DateTime.UtcNow
                });
        }

        public ExchangeFile Update(int userId, int fileId, int fileCategoryId, string description, string tags,
            bool accessDenied, bool allowViewAnonymousUsers)
        {
            var file = _exchangeFileRepository
                .FindBy(f => f.UserId == userId && f.FileId == fileId)
                .SingleOrDefault();
            if (file == null)
                throw new ArgumentException(string.Format("exchangeFile not exists. UserId: {0}. FileId: {1}", userId,
                    fileId));
            file.FileCategoryId = fileCategoryId;
            file.Description = description;
            file.Tags = tags;
            file.AccessDenied = accessDenied;
            file.AllowViewAnonymousUsers = allowViewAnonymousUsers;
            file.ModifyDate = DateTime.UtcNow;
            return file;
        }



        public void RemoveUserFile(int userId, int fileId)
        {
            var file = _exchangeFileRepository
                .FindBy(f => f.UserId == userId && f.FileId == fileId)
                .SingleOrDefault();
            if (file == null)
                throw new ArgumentException(string.Format("exchangeFile not exists. UserId: {0}. FileId: {1}", userId, fileId));
            _exchangeFileRepository.Delete(file);
        }
    }
}