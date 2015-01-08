using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace FileExchange.Infrastructure.CustomHtmlHelpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString RemoveHtmlTags(this HtmlHelper html,string htmlContent,int? displayLength=null)
        {
            string content = Regex.Replace(htmlContent, @"<[^>]*>", String.Empty);
            if (displayLength.HasValue)
                content = content.Length > displayLength.Value ? content.Remove(displayLength.Value) : content;
            return MvcHtmlString.Create(content);
        }

        public static string MenuIsActive(this HtmlHelper htmlHelper, string menuActionName, string menuControllerName)
        {
            string controllerName =
                (string)htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            string actionName =
                (string)htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string result = String.Empty;
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