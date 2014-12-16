using System.Collections.Generic;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core.Services
{
    public interface IFileCategoriesService
    {
        List<FileCategories> GetAll();
    }
}