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
    public partial class AccountController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected AccountController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult Login()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Login);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AccountController Actions { get { return MVC.Account; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Account";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Account";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Login = "Login";
            public readonly string LogOff = "LogOff";
            public readonly string UserBanned = "UserBanned";
            public readonly string Register = "Register";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Login = "Login";
            public const string LogOff = "LogOff";
            public const string UserBanned = "UserBanned";
            public const string Register = "Register";
        }


        static readonly ActionParamsClass_Login s_params_Login = new ActionParamsClass_Login();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Login LoginParams { get { return s_params_Login; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Login
        {
            public readonly string returnUrl = "returnUrl";
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_Register s_params_Register = new ActionParamsClass_Register();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Register RegisterParams { get { return s_params_Register; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Register
        {
            public readonly string model = "model";
            public readonly string captchaValue = "captchaValue";
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
                public readonly string _ChangePasswordPartial = "_ChangePasswordPartial";
                public readonly string _SetPasswordPartial = "_SetPasswordPartial";
                public readonly string ExternalLoginConfirmation = "ExternalLoginConfirmation";
                public readonly string Login = "Login";
                public readonly string Manage = "Manage";
                public readonly string Register = "Register";
                public readonly string UserBanned = "UserBanned";
            }
            public readonly string _ChangePasswordPartial = "~/Views/Account/_ChangePasswordPartial.cshtml";
            public readonly string _SetPasswordPartial = "~/Views/Account/_SetPasswordPartial.cshtml";
            public readonly string ExternalLoginConfirmation = "~/Views/Account/ExternalLoginConfirmation.cshtml";
            public readonly string Login = "~/Views/Account/Login.cshtml";
            public readonly string Manage = "~/Views/Account/Manage.cshtml";
            public readonly string Register = "~/Views/Account/Register.cshtml";
            public readonly string UserBanned = "~/Views/Account/UserBanned.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_AccountController : FileExchange.Controllers.AccountController
    {
        public T4MVC_AccountController() : base(Dummy.Instance) { }

        [NonAction]
        partial void LoginOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string returnUrl);

        [NonAction]
        public override System.Web.Mvc.ActionResult Login(string returnUrl)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Login);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "returnUrl", returnUrl);
            LoginOverride(callInfo, returnUrl);
            return callInfo;
        }

        [NonAction]
        partial void LoginOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, FileExchange.Models.LoginModel model, string returnUrl);

        [NonAction]
        public override System.Web.Mvc.ActionResult Login(FileExchange.Models.LoginModel model, string returnUrl)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Login);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "returnUrl", returnUrl);
            LoginOverride(callInfo, model, returnUrl);
            return callInfo;
        }

        [NonAction]
        partial void LogOffOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult LogOff()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LogOff);
            LogOffOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void UserBannedOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult UserBanned()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UserBanned);
            UserBannedOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void RegisterOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Register()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Register);
            RegisterOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void RegisterOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, FileExchange.Models.RegisterModel model, string captchaValue);

        [NonAction]
        public override System.Web.Mvc.ActionResult Register(FileExchange.Models.RegisterModel model, string captchaValue)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Register);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "captchaValue", captchaValue);
            RegisterOverride(callInfo, model, captchaValue);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
