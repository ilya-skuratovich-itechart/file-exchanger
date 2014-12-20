using System.Collections.Generic;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core.Services
{
    public interface IExchangeFileService
    {
        List<ExchangeFile> GetUserFiles(int userId);

        ExchangeFile Add(int userId, int fileCategoryId, string description, string uniqFileName, string origFileName,
            string tags, bool accessDenied, bool allowViewAnonymousUsers);

        List<ExchangeFile> GetAll();

        List<ExchangeFile> GetUserFilesPaged(int userId, int startRecNum, int pageLenght, out int totalRecords);

        IEnumerable<ExchangeFile> GetCategoryFiles(int fileCategoryId, bool isAuthorizedUser);

        ExchangeFile GetFilteredFile(int fileId, bool isAuthorizedUser);

        IEnumerable<ExchangeFile> GetFilteredCategoryFilesPaged(int fileCategoryId,bool isAuthorizedUser, int startRecNum, int pageLenght, out int totalRecords);

        ExchangeFile GetUserFile(int fileId, int userId);

        ExchangeFile Update(int userId,int fileId, int fileCategoryId, string description, string tags, bool accessDenied, bool allowViewAnonymousUsers);

        void RemoveUserFile(int userId, int fileId);
    }
}