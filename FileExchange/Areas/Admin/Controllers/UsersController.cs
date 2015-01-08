using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using Autofac;
using FileExchange.Areas.Admin.Models;
using FileExchange.Core.DAL.Entity;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.EmailSender;
using FileExchange.Infrastructure.FileHelpers;
using FileExchange.Infrastructure.ModelBinders;
using FileExchange.Infrastructure.UserSecurity;
using FileExchange.Infrastructure.ViewsHelpers;
using FileExchange.Models.DataTable;
using WebMatrix.WebData;

namespace FileExchange.Areas.Admin.Controllers
{
    public partial class UsersController : Controller
    {
        private IUnitOfWork _unitOfWork { get; set; }
        private IUserProfileService _userProfileService { get; set; }

        private IUserRolesService _userRolesService { get; set; }

        private IUserInRolesService _userInRolesService { get; set; }

        private IMailer _mailer { get; set; }

        private IFileProvider _fileProvider { get; set; }
        private IWebSecurity _webSecurity { get; set; }
        private IViewRenderWrapper _viewRenderHelper { get; set; }
        public UsersController(IUnitOfWork unitOfWork,
            IUserProfileService userProfileService,
            IUserRolesService userRolesService,
            IUserInRolesService userInRolesService,
             IMailer mailer,
            IFileProvider fileProvider,
            IWebSecurity webSecurity,
            IViewRenderWrapper viewRenderHelper)
        {
            _unitOfWork = unitOfWork;
            _userProfileService = userProfileService;
            _userRolesService = userRolesService;
            _userInRolesService = userInRolesService;
            _mailer = mailer;
            _fileProvider = fileProvider;
            _webSecurity = webSecurity;
            _viewRenderHelper = viewRenderHelper;
        }

        //
        // GET: /Admin/Users/

        public virtual ActionResult ViewUsers()
        {
            return View();
        }

        public virtual ActionResult EditUser(int userId)
        {
            EditUserViewModel userModel =
                AutoMapper.Mapper.Map<EditUserViewModel>(_userProfileService.GetUserById(userId));
            userModel.UserRoles = AutoMapper.Mapper.Map<IEnumerable<UserRolesModel>>(_userRolesService.GetAll());
            return View(userModel);
        }

        [HttpPost]
        public virtual ActionResult EditUser(EditUserViewModel userModel)
        {

            if (!ModelState.IsValid)
            {
                userModel.UserRoles = AutoMapper.Mapper.Map<IEnumerable<UserRolesModel>>(_userRolesService.GetAll());
                userModel.SelectedUserRoles =
                    AutoMapper.Mapper.Map<IEnumerable<UserRolesModel>>(
                        _userInRolesService.GetUserInRoles(userModel.UserId));
                return View(userModel);
            }
            _unitOfWork.BeginTransaction();
            UserProfile userProfile = _userProfileService.GetUserById(userModel.UserId);
            if (userProfile == null)
                throw new Exception(string.Format("userProfile not exists. UserId:{0}", userModel.UserId));
            userProfile.MaxDonwloadSpeedKbps = userModel.MaxDonwloadSpeedKbps;
            userProfile.FileMaxSizeKbps = userModel.FileMaxSizeKbps;
            _userInRolesService.UpdateUserInRoles(userProfile.UserId, userModel.RolesIds==null?new List<int>() : userModel.RolesIds.ToList());
            _userProfileService.Update(userProfile);
            _unitOfWork.SaveChanges();
            _unitOfWork.CommitTransaction();
            return RedirectToAction(MVC.Admin.Users.ActionNames.ViewUsers);

        }

        public virtual JsonResult ChangeUserPassword(int userId)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                    var user = _userProfileService.GetUserById(userId);
                    if (user == null)
                        throw new Exception(string.Format("User not exists. UserId: {0}", userId));
                    string resetToken = _webSecurity.GeneratePasswordResetToken(user.UserName, 5);
                    string newPassword = System.Web.Security.Membership.GeneratePassword(10, 2);
                    _webSecurity.ResetPassword(resetToken, newPassword);
                    string templateText = _viewRenderHelper.RenderViewToString(
                        MVC.Admin.EmailTemplates.Views.PasswordChanged,
                        MVC.Admin.EmailTemplates.Views._layout,
                        new {UserName = user.UserName, Password = newPassword});
                    _mailer.SendEmailTo(user.UserEmail, "Password has been changed", templateText);
                _unitOfWork.CommitTransaction();
                return Json(new {Success = true},JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        [HttpPost]
        public virtual JsonResult ViewUserDataTableFilter(
            [ModelBinder(typeof (DataTablesBinder))] DefaultDataTablesRequest param)
        {
            try
            {
                int totalRecords = 0;
                IEnumerable<UserProfile> users = _userProfileService.GetUsersPaged(param.Start, param.Length,
                    out totalRecords);
                var resultUserProfiles = from val in users
                    select new[]
                    {
                        Convert.ToString(val.UserId),
                        string.Empty,
                        val.UserName,
                        val.UserEmail,
                        (val.Roles != null && val.Roles.Any())
                            ? val.Roles.Select(s => s.Role.RoleName)
                                .Aggregate((i, j) => i + ',' + j)
                            : string.Empty
                    };
                var result = new
                {
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = resultUserProfiles.Count(),
                    aaData = resultUserProfiles

                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                throw;
            }
        }

    }
}
