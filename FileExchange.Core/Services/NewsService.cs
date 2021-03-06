﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FileExchange.Core.UOW;
using System.ServiceModel.Syndication;
using FileExchange.Core.DAL.Entity;
using FileExchange.Core.DAL.Repository;

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
            int totalRecorsCount;
            return _repository.GetPaged(n=>n.NewsId,1, count, out totalRecorsCount)
                .ToList();
        }


        public IEnumerable<News> GetPaged(int pageNumber, int pageSize,out int pageCount)
        {
            return _repository.GetPaged(n => n.NewsId, pageNumber, pageSize, out pageCount);
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
            _repository.Update(news);
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
