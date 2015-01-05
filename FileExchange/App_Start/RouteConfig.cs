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
             name: "UserBanned",
             url: string.Format("Account/UserBanned"),
             defaults: new { controller =MVC.Account.Name, action =  MVC.Account.ActionNames.UserBanned });

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
                name: "News",
                url: string.Format("{0}/{1}/{{newsId}}", MVC.News.Name, MVC.News.ActionNames.News),
                defaults:
                    new
                    {
                        controller = MVC.News.Name,
                        action = MVC.News.ActionNames.News
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
                defaults:
                    new
                    {
                        controller = MVC.File.Name,
                        action = MVC.File.ActionNames.EditUserFile,
                        fileId = UrlParameter.Optional
                    });

            routes.MapRoute(
                name: "DeleteUserFile",
                url: string.Format("{0}/{1}/{{fileId}}", MVC.File.Name, MVC.File.ActionNames.DeleteUserFile),
                defaults:
                    new
                    {
                        controller = MVC.File.Name,
                        action = MVC.File.ActionNames.DeleteUserFile,
                        fileId = UrlParameter.Optional
                    });

            routes.MapRoute(
                name: "ViewCategoryFiles",
                url: string.Format("{0}/{1}/{{categoryId}}", MVC.File.Name, MVC.File.ActionNames.ViewCategoryFiles),
                defaults:
                    new
                    {
                        controller = MVC.File.Name,
                        action = MVC.File.ActionNames.ViewCategoryFiles,
                        categoryId = UrlParameter.Optional
                    });


            routes.MapRoute(
                name: "ViewCategoryFilesTableFilter",
                url: string.Format("{0}/{1}", MVC.File.Name, MVC.File.ActionNames.ViewCategoryFilesTableFilter),
                defaults: new {controller = MVC.File.Name, action = MVC.File.ActionNames.ViewCategoryFilesTableFilter});


            routes.MapRoute(
                name: "ViewNews",
                url: string.Format("{0}/{1}/{{page}}/{{pageSize}}", MVC.News.Name, MVC.News.ActionNames.ViewNews),
                defaults:
                    new
                    {
                        controller = MVC.News.Name,
                        action = MVC.News.ActionNames.ViewNews,
                        page = UrlParameter.Optional,
                        pageSize = UrlParameter.Optional
                    });

            routes.MapRoute(
                name: "DownloadFileExchange",
                url: string.Format("{0}/{1}/{{fileId}}", MVC.File.Name, MVC.File.ActionNames.DownloadFile),
                defaults:
                    new
                    {
                        controller = MVC.File.Name,
                        action = MVC.File.ActionNames.DownloadFile,
                        fileId = UrlParameter.Optional
                    });

            routes.MapRoute(
                name: "ViewFile",
                url: string.Format("{0}/{1}/{{fileId}}", MVC.File.Name, MVC.File.ActionNames.ViewFile),
                defaults:
                    new
                    {
                        controller = MVC.File.Name,
                        action = MVC.File.ActionNames.ViewFile,
                        fileId = UrlParameter.Optional
                    });

            routes.MapRoute(
                name: "ViewFileComments",
                url: string.Format("{0}/{1}/{{fileId}}", MVC.File.Name, MVC.File.ActionNames.ViewFileComments),
                defaults:
                    new
                    {
                        controller = MVC.File.Name,
                        action = MVC.File.ActionNames.ViewFileComments,
                        fileId = UrlParameter.Optional
                    });

            routes.MapRoute(
                name: "AddFileComment",
                url: string.Format("{0}/{1}", MVC.File.Name, MVC.File.ActionNames.AddComment),
                defaults:
                    new
                    {
                        controller = MVC.File.Name,
                        action = MVC.File.ActionNames.AddComment
                    });


            routes.MapRoute(
                name: "SubscribeFileNotification",
                url: string.Format("{0}/{1}/{{fileId}}", MVC.File.Name, MVC.File.ActionNames.SubscribeFileNotification),
                defaults:
                    new
                    {
                        controller = MVC.File.Name,
                        action = MVC.File.ActionNames.SubscribeFileNotification,
                        fileId = UrlParameter.Optional
                    });

            routes.MapRoute(
                name: "UnscribeFileNotification",
                url: string.Format("{0}/{1}/{{fileId}}", MVC.File.Name, MVC.File.ActionNames.UnscribeFileNotification),
                defaults:
                    new
                    {
                        controller = MVC.File.Name,
                        action = MVC.File.ActionNames.UnscribeFileNotification,
                        fileId = UrlParameter.Optional
                    });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional});

        }
    }
}