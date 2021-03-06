﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileExchange.Core.Data;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.Infrastructure.Configuration;
using FileExchange.Infrastructure.FileHelpers;
using FileExchange.Models;
using FileExchange.Infrastructure.PageList;
using FileExchange.Infrastructure.UserSecurity;

namespace FileExchange.Controllers
{
   
    public partial class NewsController : Controller
    {
        private IUnitOfWork _unitOfWork { get; set; }
        private INewsService _newsService { get; set; }
        private IWebSecurity _webSecurity { get; set; }
        private IFileProvider _fileProvider { get; set; }

        public NewsController(IUnitOfWork unitOfWork, INewsService newsService, IWebSecurity webSecurity,IFileProvider fileProvider)
        {
            _unitOfWork = unitOfWork;
            _newsService = newsService;
            _webSecurity = webSecurity;
            _fileProvider = fileProvider;
        }

        [HttpGet]
        public virtual ActionResult GetLastNews()
        {
            var model = new LastNewsModel();
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(UserRoleNames.Admin) || User.IsInRole(UserRoleNames.Moderator))
                {
                    model.AllowEdit = true;
                }
            }
            var news = _newsService.GetLastNews(10);
            model.News = AutoMapper.Mapper.Map<List<FileExchange.Models.EditNewsModel>>(news);
            model.News = model.News ?? new List<EditNewsModel>();
            return PartialView(MVC.News.Views.ViewNames._lastNews, model);
        }

        [HttpGet]
        [Authorize(Roles = UserRoleNames.EditNewsAllowRoles)]
        public virtual ActionResult AddNews()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = UserRoleNames.EditNewsAllowRoles)]
        public virtual ActionResult AddNews(CreateNewsModel news)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqFileName = Guid.NewGuid().ToString() + Path.GetExtension(news.File.FileName);
                   _unitOfWork.BeginTransaction();
                        _newsService.Add(news.Header, news.Text, uniqFileName, news.File.FileName);
                        var path =
                            this.HttpContext.Server.MapPath(string.Format("~/{0}",
                                Path.Combine(ConfigHelper.FilesFolder, uniqFileName)));
                        news.File.SaveAs(path);
                        _unitOfWork.SaveChanges();
                       _unitOfWork.CommitTransaction();
                        _unitOfWork.Dispose();
                    return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
                }
                else
                    return View(news);
            }
            catch (Exception exc)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public virtual ActionResult ViewNews(int page = 1, int pageSize = 10)
        {
            int totalItemsCount;
            IEnumerable<ViewNewsViewModel> news = AutoMapper.Mapper.Map<IEnumerable<ViewNewsViewModel>>(_newsService.GetPaged(page, pageSize,out totalItemsCount));
            Infrastructure.PageList.PagedList<ViewNewsViewModel> pageList = new PagedList<ViewNewsViewModel>(news, page,
                pageSize, totalItemsCount);
            return View(pageList);
        }

        public virtual ActionResult News(int newsId)
        {
            ViewNewsViewModel news = AutoMapper.Mapper.Map<ViewNewsViewModel>(_newsService.GetById(newsId));
            return View(news);
        }

        [HttpGet]
        [Authorize(Roles = UserRoleNames.EditNewsAllowRoles)]
        public virtual ActionResult EditNews(int newsid)
        {
            EditNewsModel model = AutoMapper.Mapper.Map<FileExchange.Models.EditNewsModel>(_newsService.GetById(newsid));
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = UserRoleNames.EditNewsAllowRoles)]
        public virtual ActionResult EditNews(EditNewsModel news)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.BeginTransaction();
                    string fileNameToDelete=string.Empty;
                    if (news.File.ContentLength > 0)
                    {
                        fileNameToDelete =_newsService.GetById(1).UniqImageName;
                        news.UniqImageName = Guid.NewGuid().ToString() + Path.GetExtension(news.File.FileName);
                    }
                    _newsService.Update(news.NewsId, news.Header, news.Text, news.UniqImageName, news.OrigImageName);
                    if (news.File.ContentLength > 0)
                    {
                        var newFilePath =
                            this.HttpContext.Server.MapPath(string.Format("~/{0}",
                                Path.Combine(ConfigHelper.FilesFolder, news.UniqImageName)));

                        var oldFilePath =
                            this.HttpContext.Server.MapPath(string.Format("~/{0}",
                                Path.Combine(ConfigHelper.FilesFolder, fileNameToDelete)));
                        _fileProvider.SaveAs(news.File, newFilePath);
                        _fileProvider.Delete(oldFilePath);
                    }
                    _unitOfWork.SaveChanges();
                    _unitOfWork.CommitTransaction();
                    return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
                }
                else
                    return View(news);
            }
            catch (Exception exc)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

    }
}
