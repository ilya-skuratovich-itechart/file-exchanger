using System;
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
                AutoMapper.Mapper.Map(news, model.News);
                model.News = model.News ?? new List<NewsModel>();
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
        public virtual ActionResult AddNews(NewsModel news)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqFileName = Guid.NewGuid().ToString();
                    string fileExtension = Path.GetExtension(news.File.FileName);
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        //to do
                        _newsService.Add(news.Header, Path.Combine(ConfigHelper.FilesFolder, news.File.FileName),
                            uniqFileName);
                        var fileName = Path.GetFileName(news.File.FileName);
                        var path =
                            System.Web.HttpContext.Current.Server.MapPath(string.Format("~/{0}",
                                Path.Combine(ConfigHelper.FilesFolder, uniqFileName + fileExtension)));
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
                _unitOfWork.Dispose();
                throw;
            }
        }


    }
}
