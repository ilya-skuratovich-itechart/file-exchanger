using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;

namespace FileExchange.Controllers
{
    public partial class HomeController : Controller
    {
        private INewsService _newsService;
        private IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork, INewsService newsService)
        {
            _unitOfWork = unitOfWork;
            _newsService = newsService;
        }
        public virtual ActionResult Index()
        {
            _newsService.GetAll();
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public virtual ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
