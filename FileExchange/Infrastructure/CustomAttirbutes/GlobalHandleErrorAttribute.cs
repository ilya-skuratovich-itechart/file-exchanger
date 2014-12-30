using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Autofac;
using FileExchange.Infrastructure.LoggerManager;

namespace FileExchange.Infrastructure.CustomAttirbutes
{
    public class GlobalHandleErrorAttribute : HandleErrorAttribute,IFilter
    {
        private ILogger _logger { get; set; }

        public GlobalHandleErrorAttribute()
        {
            _logger=AutofacConfig.ApplicationContainer.Container.Resolve<ILogger>();
        }
        public override void OnException(ExceptionContext filterContext)
        {
            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            {
                return;
            }

            if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                return;
            }

            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest" || filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = "Application error"
                    }
                };
            }
            else
            {
                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                filterContext.Result = new ViewResult
                {
                    ViewName = MVC.Error.Views.Error,
                    MasterName = MVC.Shared.Views._Layout,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
            }
            _logger.Error(filterContext.Exception);
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}