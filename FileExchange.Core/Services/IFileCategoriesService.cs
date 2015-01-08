using System.Collections.Generic;
using FileExchange.Core.DAL.Entity;

namespace FileExchange.Core.Services
{
    public interface IFileCategoriesService
    {
        List<FileCategories> GetAll();
    }
}