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
        private IGenericRepository<News> _exchangeFileRepository;

        public NewsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            BootStrap.Container.Resolve<IGenericRepository<ExchangeFile>>();
            _exchangeFileRepository.InitializeDbContext(unitOfWork.DbContext);
        }

        public List<News> GetAll()
        {
            return _exchangeFileRepository
                .GetAll()
                .ToList();
        }

        public News GetById(int newsId)
        {
            return _exchangeFileRepository
                 .FindBy(n=>n.NewsId==newsId)
                 .SingleOrDefault();
        }

        public News Add(string header, string text, string imgPath)
        {
            var news = new News()
            {
                Header = header,
                Text = text,
                CreateDate = DateTime.UtcNow
            };
            return _exchangeFileRepository.Add(news);
        }

        public News Update(int newsId,string header, string text, string imgPath)
        {
            var news = _exchangeFileRepository
                  .FindBy(n => n.NewsId == newsId)
                  .SingleOrDefault();
            if (news==null)
                throw new ArgumentException(string.Format("news object not found. NewsId:{0}",news));
            news.ModifyDate = DateTime.UtcNow;
            news.Header = header;
            news.Text = text;
            news.ImagePath = imgPath;
            _exchangeFileRepository.Edit(news);
            return news;
        }

        public void Remove(int newsId)
        {
            var news = _exchangeFileRepository
                 .FindBy(n => n.NewsId == newsId)
                 .SingleOrDefault();
            if (news == null)
                throw new ArgumentException(string.Format("news object not found. NewsId:{0}", news));
            _exchangeFileRepository.Delete(news);
        }
    }
}
