// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
#pragma warning disable 1591, 3008, 3009
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace FileExchange.Controllers
{
    public partial class NewsController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected NewsController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult EditNews()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditNews);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public NewsController Actions { get { return MVC.News; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "News";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "News";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string GetLastNews = "GetLastNews";
            public readonly string AddNews = "AddNews";
            public readonly string EditNews = "EditNews";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string GetLastNews = "GetLastNews";
            public const string AddNews = "AddNews";
            public const string EditNews = "EditNews";
        }


        static readonly ActionParamsClass_AddNews s_params_AddNews = new ActionParamsClass_AddNews();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AddNews AddNewsParams { get { return s_params_AddNews; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AddNews
        {
            public readonly string news = "news";
        }
        static readonly ActionParamsClass_EditNews s_params_EditNews = new ActionParamsClass_EditNews();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_EditNews EditNewsParams { get { return s_params_EditNews; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_EditNews
        {
            public readonly string newsid = "newsid";
            public readonly string news = "news";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _lastNews = "_lastNews";
                public readonly string AddNews = "AddNews";
                public readonly string EditNews = "EditNews";
            }
            public readonly string _lastNews = "~/Views/News/_lastNews.cshtml";
            public readonly string AddNews = "~/Views/News/AddNews.cshtml";
            public readonly string EditNews = "~/Views/News/EditNews.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_NewsController : FileExchange.Controllers.NewsController
    {
        public T4MVC_NewsController() : base(Dummy.Instance) { }

        [NonAction]
        partial void GetLastNewsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult GetLastNews()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetLastNews);
            GetLastNewsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void AddNewsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult AddNews()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddNews);
            AddNewsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void AddNewsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, FileExchange.Models.CreateNewsModel news);

        [NonAction]
        public override System.Web.Mvc.ActionResult AddNews(FileExchange.Models.CreateNewsModel news)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddNews);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "news", news);
            AddNewsOverride(callInfo, news);
            return callInfo;
        }

        [NonAction]
        partial void EditNewsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int newsid);

        [NonAction]
        public override System.Web.Mvc.ActionResult EditNews(int newsid)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditNews);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "newsid", newsid);
            EditNewsOverride(callInfo, newsid);
            return callInfo;
        }

        [NonAction]
        partial void EditNewsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, FileExchange.Models.EditNewsModel news);

        [NonAction]
        public override System.Web.Mvc.ActionResult EditNews(FileExchange.Models.EditNewsModel news)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditNews);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "news", news);
            EditNewsOverride(callInfo, news);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009