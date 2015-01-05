using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Autofac;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;

namespace FileExchange.Infrastructure.Filtres
{
    public class BannedUserFilter : AuthorizeAttribute
    {
        private Dictionary<string,List<string>> _actionsToSkip { get; set; }
        private IUserProfileService _userProfileService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionsToSkip">key - controller name, value - actionName</param>
        public BannedUserFilter( Dictionary<string,List<string>> actionsToSkip)
        {
            _userProfileService=AutofacConfig.ApplicationContainer.Container.Resolve<IUserProfileService>();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (_actionsToSkip != null
                && _actionsToSkip.ContainsKey(filterContext.RouteData.Values["controller"].ToString())
                && _actionsToSkip[filterContext.RouteData.Values["controller"].ToString()].Any(a =>
                    String.Compare(a, filterContext.ActionDescriptor.ActionName, true) == 0))
            {
                return;
            }
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                UserProfile userProfile = _userProfileService.GetUserByName(filterContext.HttpContext.User.Identity.Name);
                if (!userProfile.Roles.Any(r => r.Role.RoleType == UserRoleTypes.ActiveUser))
                {
                    filterContext.Result = new RedirectResult("~/Account/UserBanned");
                }
            }
        }
    }
}