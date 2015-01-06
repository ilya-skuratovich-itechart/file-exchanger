using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using FileExchange.Core;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Data;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using FileExchange.Filters;
using FileExchange.Infrastructure.Captcha;
using FileExchange.Infrastructure.UserSecurity;
using FileExchange.Models;

namespace FileExchange.Controllers
{
    [Authorize]
    public partial class AccountController : Controller
    {
        private IUserProfileService _userProfileService { get; set; }
        private IUserInRolesService _userInRolesService { get; set; }
        private IUnitOfWork _unitOfWork { get; set; }
        private IWebSecurity _webSecurity { get; set; }
        private ICaptcha _captchaHelper { get; set; }

        public AccountController(IUnitOfWork unitOfWork, IUserProfileService userProfileService, IUserInRolesService userInRolesService, IWebSecurity webSecurity,
           ICaptcha captchaHelper)
        {
            _userProfileService = userProfileService;
            _userInRolesService = userInRolesService;
            _unitOfWork = unitOfWork;
            _webSecurity = webSecurity;
            _captchaHelper = captchaHelper;
        }
        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && _webSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LogOff()
        {
            _webSecurity.Logout();
            return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
        }

        public virtual ActionResult UserBanned()
        {
            _webSecurity.Logout();
            return View(MVC.Account.Views.ViewNames.UserBanned);
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public virtual ActionResult Register()
        {
            return View(MVC.Account.Views.ViewNames.Register);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Register(RegisterModel model, string captchaValue)
        {
            if (!_captchaHelper.IsValidCaptchaValue(captchaValue))
                ModelState.AddModelError("", "Incorrect captcha");
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    _webSecurity.CreateUserAndAccount(model.UserName, model.Password,
                        propertyValues: new
                        {
                            UserName = model.UserName,
                            UserEmail = model.Email
                        });
                    _userInRolesService.AddUserToRole(_webSecurity.GetUserId(model.UserName),
                        UserRoleTypes.ActiveUser);
                    _unitOfWork.SaveChanges();
                    _webSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
