﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileExchange.Areas.Admin.Models;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.ModelBinders;
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


        public UsersController(IUnitOfWork unitOfWork,
            IUserProfileService userProfileService,
            IUserRolesService userRolesService,
            IUserInRolesService userInRolesService)
        {
            _unitOfWork = unitOfWork;
            _userProfileService = userProfileService;
            _userRolesService = userRolesService;
            _userInRolesService = userInRolesService;
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
        public ActionResult EditUser(EditUserViewModel userModel)
        {

            if (!ModelState.IsValid)
            {
                userModel.UserRoles = AutoMapper.Mapper.Map<IEnumerable<UserRolesModel>>(_userRolesService.GetAll());
                userModel.SelectedUserRoles =
                    AutoMapper.Mapper.Map<IEnumerable<UserRolesModel>>(_userInRolesService.GetUserInRoles(userModel.UserId));
                return View(userModel);
            }
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                UserProfile userProfile = _userProfileService.GetUserById(userModel.UserId);
                if (userProfile==null)
                    throw new Exception(string.Format("userProfile not exists. UserId:{0}",userModel.UserId));

                userProfile.MaxDonwloadSpeedKbps = userModel.MaxDonwloadSpeedKbps;
                userProfile.FileMaxSizeKbps = userModel.FileMaxSizeKbps;
               
                _userInRolesService.UpdateUserInRoles(userProfile.UserId, userModel.RolesIds.ToList());
                _userProfileService.Update(userProfile);
                _unitOfWork.SaveChanges();
                transaction.Complete();
            }
            return RedirectToAction(MVC.Admin.Users.ActionNames.ViewUsers);

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