using System.Collections.Generic;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core.Services
{
    public interface INewsService
    {
        List<News> GetAll();

        News GetById(int newsId);

        News Add(string header, string text,string imgPath);

        News Update(int newsId,string header, string text, string imgPath);

        void Remove(int newsId);

    }
}