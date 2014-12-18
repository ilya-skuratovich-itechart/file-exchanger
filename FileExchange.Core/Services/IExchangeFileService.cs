﻿using System.Collections.Generic;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core.Services
{
    public interface IExchangeFileService
    {
        List<ExchangeFile> GetUserFiles(int userId);

        List<ExchangeFile> GetAll();

        List<ExchangeFile> GetUserFilesPaged(int userId, int startRecNum, int pageLenght, out int totalRecords);

        List<ExchangeFile> GetCategoryFiles(int fileCategoryId);

        ExchangeFile GetUserFile(int fileId, int userId);

        ExchangeFile Add(int userId, int fileCategoryId,string description, string uniqFileName,string origFileName, string tags, bool accessDenied, bool allowViewAnonymousUsers);

        ExchangeFile Update(int userId,int fileId, int fileCategoryId, string description, string tags, bool accessDenied, bool allowViewAnonymousUsers);

        void RemoveUserFile(int userId, int fileId);
    }
}