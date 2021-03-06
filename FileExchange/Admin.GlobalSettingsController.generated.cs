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
namespace FileExchange.Areas.Admin.Controllers
{
    public partial class GlobalSettingsController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected GlobalSettingsController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult UpdateSetting()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateSetting);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult ViewSettingsTableFilter()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ViewSettingsTableFilter);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public GlobalSettingsController Actions { get { return MVC.Admin.GlobalSettings; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Admin";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "GlobalSettings";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "GlobalSettings";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string ViewSettings = "ViewSettings";
            public readonly string UpdateSetting = "UpdateSetting";
            public readonly string ViewSettingsTableFilter = "ViewSettingsTableFilter";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string ViewSettings = "ViewSettings";
            public const string UpdateSetting = "UpdateSetting";
            public const string ViewSettingsTableFilter = "ViewSettingsTableFilter";
        }


        static readonly ActionParamsClass_UpdateSetting s_params_UpdateSetting = new ActionParamsClass_UpdateSetting();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateSetting UpdateSettingParams { get { return s_params_UpdateSetting; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateSetting
        {
            public readonly string settingId = "settingId";
            public readonly string settingModel = "settingModel";
        }
        static readonly ActionParamsClass_ViewSettingsTableFilter s_params_ViewSettingsTableFilter = new ActionParamsClass_ViewSettingsTableFilter();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ViewSettingsTableFilter ViewSettingsTableFilterParams { get { return s_params_ViewSettingsTableFilter; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ViewSettingsTableFilter
        {
            public readonly string param = "param";
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
                public readonly string UpdateSetting = "UpdateSetting";
                public readonly string ViewSettings = "ViewSettings";
            }
            public readonly string UpdateSetting = "~/Areas/Admin/Views/GlobalSettings/UpdateSetting.cshtml";
            public readonly string ViewSettings = "~/Areas/Admin/Views/GlobalSettings/ViewSettings.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_GlobalSettingsController : FileExchange.Areas.Admin.Controllers.GlobalSettingsController
    {
        public T4MVC_GlobalSettingsController() : base(Dummy.Instance) { }

        [NonAction]
        partial void ViewSettingsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult ViewSettings()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ViewSettings);
            ViewSettingsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void UpdateSettingOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int settingId);

        [NonAction]
        public override System.Web.Mvc.ActionResult UpdateSetting(int settingId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateSetting);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "settingId", settingId);
            UpdateSettingOverride(callInfo, settingId);
            return callInfo;
        }

        [NonAction]
        partial void UpdateSettingOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, FileExchange.Areas.Admin.Models.GlobalSettingViewModel settingModel);

        [NonAction]
        public override System.Web.Mvc.ActionResult UpdateSetting(FileExchange.Areas.Admin.Models.GlobalSettingViewModel settingModel)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateSetting);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "settingModel", settingModel);
            UpdateSettingOverride(callInfo, settingModel);
            return callInfo;
        }

        [NonAction]
        partial void ViewSettingsTableFilterOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, FileExchange.Models.DataTable.DefaultDataTablesRequest param);

        [NonAction]
        public override System.Web.Mvc.JsonResult ViewSettingsTableFilter(FileExchange.Models.DataTable.DefaultDataTablesRequest param)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ViewSettingsTableFilter);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "param", param);
            ViewSettingsTableFilterOverride(callInfo, param);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
