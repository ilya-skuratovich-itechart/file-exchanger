﻿using System.Collections.Generic;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core.Services
{
    public interface IExchangeFileService
    {
        List<ExchangeFile> GetUserFiles(int userId);

        List<ExchangeFile> GetAll();

        List<ExchangeFile> GetCategoryFiles(int fileCategoryId);

        ExchangeFile GetUserFile(int fileId, int userId);

        void Add(int userId, int fileCategoryId, string uniqFileName,string origFileName, string tags);

        void RemoveUserFile(int userId, int fileId);
    }
}