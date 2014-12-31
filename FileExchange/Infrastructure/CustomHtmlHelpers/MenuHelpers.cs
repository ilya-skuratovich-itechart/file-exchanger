using System.Web.Mvc;

namespace FileExchange.Infrastructure.CustomHtmlHelpers
{
    public static class MenuHelpers
    {
        public static string IsActive(this HtmlHelper htmlHelper, string menuActionName, string menuControllerName)
        {
            string controllerName =
                (string) htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            string actionName =
                (string) htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string result = string.Empty;
            if (MVC.File.Name == controllerName && controllerName == menuControllerName)
            {
                if ((MVC.File.ActionNames.UserFiles == actionName
                     || MVC.File.ActionNames.EditUserFile == actionName)
                    && (MVC.File.ActionNames.UserFiles == menuActionName
                        || MVC.File.ActionNames.EditUserFile == menuActionName))
                    result = "active";
            }
            else if (MVC.Home.Name == controllerName && controllerName == menuControllerName
                     && MVC.Home.ActionNames.Index == actionName && actionName == menuActionName)
            {
                result = "active";
            }
            else if (MVC.News.Name == controllerName && controllerName == menuControllerName)
            {
                if ((MVC.News.ActionNames.ViewNews == actionName
                     || MVC.News.ActionNames.News == actionName
                     || MVC.News.ActionNames.AddNews == actionName
                     || MVC.News.ActionNames.EditNews == actionName)
                    &&
                    (MVC.News.ActionNames.ViewNews == menuActionName
                     || MVC.News.ActionNames.News == menuActionName
                     || MVC.News.ActionNames.AddNews == menuActionName
                     || MVC.News.ActionNames.EditNews == menuActionName
                        ))
                    result = "active";
            }
            return result;
        }
    }
}