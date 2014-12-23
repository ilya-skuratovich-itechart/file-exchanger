using System.Web.Mvc;

namespace FileExchange.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapRoute(
               "adminpanel",
               "adminpanel",
               new
               {
                   controller = MVC.Admin.Users.Name,
                   action = MVC.Admin.Users.ActionNames.ViewUsers,
                   area = AreaName 
               },
               namespaces: new string[] { "FileExchange.Areas.Admin.Controllers" });
            #region user routes

            context.MapRoute(
                name: "ViewUsers",
                url: string.Format("admin/{0}/{1}/{2}", AreaName,MVC.Admin.Users.Name, MVC.Admin.Users.ActionNames.ViewUsers),
                defaults: new { controller = MVC.Admin.Users.Name, action = MVC.Admin.Users.ActionNames.ViewUsers, area = AreaName });

            context.MapRoute(
             name: "EditUser",
             url: string.Format("admin/{0}/{1}/{2}/{{userId}}", AreaName, MVC.Admin.Users.Name, MVC.Admin.Users.ActionNames.EditUser),
             defaults: new { 
                 controller = MVC.Admin.Users.Name,
                 action = MVC.Admin.Users.ActionNames.EditUser, 
                 area = AreaName,
                 userId = UrlParameter.Optional 
             });

            context.MapRoute(
             name: "ViewUsersTableFilter",
             url: string.Format("{0}/{1}/{2}",AreaName, MVC.Admin.Users.Name, MVC.Admin.Users.ActionNames.ViewUserDataTableFilter),
             defaults: new { controller = MVC.Admin.Users.Name, action = MVC.Admin.Users.ActionNames.ViewUserDataTableFilter, area = AreaName });
            #endregion
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }

            );
        }
    }
}
