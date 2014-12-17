using System.Collections.Generic;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core.Services
{
    public interface INewsService
    {
        List<News> GetAll();

        News GetById(int newsId);

        News Add(string header, string text, string uniqueImageName, string origImageName);

        News Update(int newsId, string header, string text, string uniqueImageName, string origImageName);

        List<News> GetLastNews(int count);

        void Remove(int newsId);
    }
}