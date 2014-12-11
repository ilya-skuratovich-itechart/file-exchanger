using System;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core.Services
{
    public interface IFileCommentService
    {
        FileComments Add(int fileId,string comment, DateTime createDate);

        void RemoveAll(int fileId);
    }
}