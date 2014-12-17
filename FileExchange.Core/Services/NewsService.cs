using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Repositories;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Services
{
    public class NewsService : INewsService
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<News> _repository;

        public NewsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = BootStrap.Container.Resolve<IGenericRepository<News>>();
            _repository.InitializeDbContext(unitOfWork.DbContext);
        }

        public List<News> GetAll()
        {
            return _repository
                .GetAll()
                .ToList();
        }

        public News GetById(int newsId)
        {
            return _repository
                 .FindBy(n=>n.NewsId==newsId)
                 .SingleOrDefault();
        }

        public News Add(string header, string text, string uniqueImageName, string origImageName)
        {
            var news = new News()
            {
                Header = header,
                Text = text,
                UniqImageName = uniqueImageName,
                OrigImageName = origImageName,
                CreateDate = DateTime.UtcNow
            };
            return _repository.Add(news);
        }

        public List<News> GetLastNews(int count)
        {
            return _repository.GetPaged(1, count)
                .ToList();
        }
        public News Update(int newsId,string header, string text, string uniqueImageName,string origImageName)
        {
            var news = _repository
                  .FindBy(n => n.NewsId == newsId)
                  .SingleOrDefault();
            if (news==null)
                throw new ArgumentException(string.Format("news object not found. NewsId:{0}",news));
            news.ModifyDate = DateTime.UtcNow;
            news.Header = header;
            news.Text = text;
            news.UniqImageName = uniqueImageName;
            news.OrigImageName = origImageName;
            _repository.Edit(news);
            return news;
        }

        public void Remove(int newsId)
        {
            var news = _repository
                 .FindBy(n => n.NewsId == newsId)
                 .SingleOrDefault();
            if (news == null)
                throw new ArgumentException(string.Format("news object not found. NewsId:{0}", news));
            _repository.Delete(news);
        }
    }
}
