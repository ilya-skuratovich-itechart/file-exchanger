using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FileExchange
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "EditNews",
                url: "News/EditNews/{newsId}",
                defaults:
                    new
                    {
                        controller = MVC.News.Name,
                        action = MVC.News.ActionNames.EditNews,
                        newsId = UrlParameter.Optional
                    });

            routes.MapRoute(
                name: "ViewFileCategories",
                url: string.Format("{0}/{1}", MVC.File.Name, MVC.File.ActionNames.FileSections),
                defaults: new {controller = MVC.File.Name, action = MVC.File.ActionNames.FileSections});

            routes.MapRoute(
                name: "AddUserFile",
                url: string.Format("{0}/{1}", MVC.File.Name, MVC.File.ActionNames.AddUserFile),
                defaults: new {controller = MVC.File.Name, action = MVC.File.ActionNames.AddUserFile});

            routes.MapRoute(
                name: "ViewUserFilesTableFilter",
                url: string.Format("{0}/{1}", MVC.File.Name, MVC.File.ActionNames.UserFilesTableFilter),
                defaults: new {controller = MVC.File.Name, action = MVC.File.ActionNames.UserFilesTableFilter});

            routes.MapRoute(
                name: "EditUserFile",
                url: string.Format("{0}/{1}/{{fileId}}", MVC.File.Name, MVC.File.ActionNames.EditUserFile),
                defaults: new { controller = MVC.File.Name, action = MVC.File.ActionNames.EditUserFile, fileId = UrlParameter.Optional });

            routes.MapRoute(
             name: "DeleteUserFile",
             url: string.Format("{0}/{1}/{{fileId}}", MVC.File.Name, MVC.File.ActionNames.DeleteUserFile),
             defaults: new { controller = MVC.File.Name, action = MVC.File.ActionNames.DeleteUserFile, fileId = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional});

        }
    }
}