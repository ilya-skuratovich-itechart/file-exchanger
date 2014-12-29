using System.Web.Mvc;

namespace FileExchange.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Admin"; }
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
                url:
                    string.Format("admin/{0}/{1}/{2}", AreaName, MVC.Admin.Users.Name,
                        MVC.Admin.Users.ActionNames.ViewUsers),
                defaults:
                    new
                    {
                        controller = MVC.Admin.Users.Name,
                        action = MVC.Admin.Users.ActionNames.ViewUsers,
                        area = AreaName
                    });

            context.MapRoute(
                name: "EditUser",
                url:
                    string.Format("admin/{0}/{1}/{2}/{{userId}}", AreaName, MVC.Admin.Users.Name,
                        MVC.Admin.Users.ActionNames.EditUser),
                defaults: new
                {
                    controller = MVC.Admin.Users.Name,
                    action = MVC.Admin.Users.ActionNames.EditUser,
                    area = AreaName,
                    userId = UrlParameter.Optional
                });

            context.MapRoute(
                name: "ViewUsersTableFilter",
                url:
                    string.Format("{0}/{1}/{2}", AreaName, MVC.Admin.Users.Name,
                        MVC.Admin.Users.ActionNames.ViewUserDataTableFilter),
                defaults:
                    new
                    {
                        controller = MVC.Admin.Users.Name,
                        action = MVC.Admin.Users.ActionNames.ViewUserDataTableFilter,
                        area = AreaName
                    });

            context.MapRoute(
                name: "ChangeUserPassword",
                url:
                    string.Format("admin/{0}/{1}/{2}/{{userId}}", AreaName, MVC.Admin.Users.Name,
                        MVC.Admin.Users.ActionNames.ChangeUserPassword),
                defaults: new
                {
                    controller = MVC.Admin.Users.Name,
                    action = MVC.Admin.Users.ActionNames.ChangeUserPassword,
                    area = AreaName,
                    userId = UrlParameter.Optional
                });
            #endregion

            #region settingRoutes
            context.MapRoute(
                name: "ViewGlobalSettings",
                url:
                    string.Format("{0}/{1}/{2}", AreaName, MVC.Admin.GlobalSettings.Name,
                        MVC.Admin.GlobalSettings.ActionNames.ViewSettings),
                defaults:
                    new
                    {
                        controller = MVC.Admin.GlobalSettings.Name,
                        action = MVC.Admin.GlobalSettings.ActionNames.ViewSettings,
                        area = AreaName
                    });

            context.MapRoute(
                name: "ViewGlobalSettingsTableFilter",
                url:
                    string.Format("{0}/{1}/{2}", AreaName, MVC.Admin.GlobalSettings.Name,
                        MVC.Admin.GlobalSettings.ActionNames.ViewSettingsTableFilter),
                defaults:
                    new
                    {
                        controller = MVC.Admin.GlobalSettings.Name,
                        action = MVC.Admin.GlobalSettings.ActionNames.ViewSettingsTableFilter,
                        area = AreaName
                    });

            context.MapRoute(
             name: "UpdateGlobalSetting",
             url:
                 string.Format("admin/{0}/{1}/{2}/{{settingId}}", AreaName, MVC.Admin.GlobalSettings.Name,
                     MVC.Admin.GlobalSettings.ActionNames.UpdateSetting),
             defaults: new
             {
                 controller = MVC.Admin.GlobalSettings.Name,
                 action = MVC.Admin.GlobalSettings.ActionNames.UpdateSetting,
                 area = AreaName,
                 settingId = UrlParameter.Optional
             });
            #endregion

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }

                );
        }
    }
}
