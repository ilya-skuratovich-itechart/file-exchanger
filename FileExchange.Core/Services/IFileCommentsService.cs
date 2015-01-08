using System;
using System.Collections.Generic;
using FileExchange.Core.DAL.Entity;

namespace FileExchange.Core.Services
{
    public interface IFileCommentService
    {
        FileComments Add(int fileId,string comment);

        IEnumerable<FileComments> GetFileComments(int fileId); 

        void RemoveAll(int fileId);
    }
}