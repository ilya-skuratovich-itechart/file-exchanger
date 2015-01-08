using System.Collections.Generic;
using System.ServiceModel.Syndication;
using FileExchange.Core.DAL.Entity;

namespace FileExchange.Core.Services
{
    public interface INewsService
    {
        List<News> GetAll();

        News GetById(int newsId);

        News Add(string header, string text, string uniqueImageName, string origImageName);

        IEnumerable<News> GetPaged(int pageNumber, int pageSize,out int pageCount);

        News Update(int newsId, string header, string text, string uniqueImageName, string origImageName);

        List<News> GetLastNews(int count);

        void Remove(int newsId);
    }
}