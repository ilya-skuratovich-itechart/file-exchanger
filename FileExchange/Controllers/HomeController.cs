using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.Infrastructure.UserSecurity;

namespace FileExchange.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

      
    }
}
