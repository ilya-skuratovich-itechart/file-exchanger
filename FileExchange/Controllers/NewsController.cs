﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Data;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.Helplers;
using FileExchange.Models;

namespace FileExchange.Controllers
{
    public partial class NewsController : Controller
    {
        private IUnitOfWork _unitOfWork { get; set; }
        private INewsService _newsService { get; set; }

        public NewsController(IUnitOfWork unitOfWork, INewsService newsService)
        {
            _unitOfWork = unitOfWork;
            _newsService = newsService;
        }

        [HttpGet]
        public virtual ActionResult GetLastNews()
        {
            try
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
            catch (Exception exc)
            {
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
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
                    string uniqFileName = Guid.NewGuid().ToString()+Path.GetExtension(news.File.FileName);
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        _newsService.Add(news.Header, news.Text, uniqFileName, news.File.FileName);
                        var path =
                            System.Web.HttpContext.Current.Server.MapPath(string.Format("~/{0}",
                                Path.Combine(ConfigHelper.FilesFolder, uniqFileName )));
                        news.File.SaveAs(path);
                        _unitOfWork.SaveChanges();
                        transaction.Complete();
                        _unitOfWork.Dispose();
                    }
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
                    string fileNameToDelete = string.Empty;
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        if (news.File.ContentLength > 0)
                        {
                            fileNameToDelete = news.UniqImageName;
                            news.UniqImageName = Guid.NewGuid().ToString() + Path.GetExtension(news.File.FileName);
                        }
                        _newsService.Update(news.NewsId, news.Header, news.Text, news.UniqImageName, news.OrigImageName);
                        if (news.File.ContentLength > 0)
                        {
                            var newFilePath =
                                System.Web.HttpContext.Current.Server.MapPath(string.Format("~/{0}",
                                    Path.Combine(ConfigHelper.FilesFolder, news.UniqImageName)));

                            var oldFilePath =
                                System.Web.HttpContext.Current.Server.MapPath(string.Format("~/{0}",
                                    Path.Combine(ConfigHelper.FilesFolder, fileNameToDelete)));

                            news.File.SaveAs(newFilePath);
                            System.IO.File.Delete(oldFilePath);
                        }
                        _unitOfWork.SaveChanges();
                        transaction.Complete();
                    }
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