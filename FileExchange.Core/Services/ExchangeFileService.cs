﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Repositories;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Services
{
    public class ExchangeFileService: IExchangeFileService
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<ExchangeFile> _exchangeFileRepository;

        public ExchangeFileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            BootStrap.Container.Resolve<IGenericRepository<ExchangeFile>>();
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
              .FindBy(f => f.UserId == userId && f.FileId==fileId)
              .SingleOrDefault();
        }

        public void Add(int userId, int fileCategoryId, string filePath, string tags)
        {
            _exchangeFileRepository.Add(
                new ExchangeFile()
                {
                    FileCategoryId = fileCategoryId,
                    FilePath = filePath,
                    Tags = tags,
                    CreateDate = DateTime.UtcNow
                });
        }

        public void RemoveUserFile(int userId, int fileId)
        {
            var file = _exchangeFileRepository
                .FindBy(f => f.UserId == userId && f.FileId == fileId)
                .SingleOrDefault();
            if (file==null)
                throw new ArgumentException(string.Format("exchangeFile not exists. UserId: {0}. FileId: {1}",userId,fileId));
            _exchangeFileRepository.Delete(file);
        }
    }
}