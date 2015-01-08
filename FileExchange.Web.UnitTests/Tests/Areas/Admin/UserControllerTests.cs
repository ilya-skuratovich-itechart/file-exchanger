using System.Collections.Generic;
using System.Web.Mvc;
using FileExchange.Areas.Admin.Controllers;
using FileExchange.Areas.Admin.Models;
using FileExchange.Controllers;
using FileExchange.Core.DAL.Entity;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.EmailSender;
using FileExchange.Infrastructure.Captcha;
using FileExchange.Infrastructure.FileHelpers;
using FileExchange.Infrastructure.UserSecurity;
using FileExchange.Infrastructure.ViewsHelpers;
using FileExchange.Models.DataTable;
using FileExchange.Web.UnitTests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FileExchange.Web.UnitTests.Tests.Areas.Admin
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock { get; set; }
        private Mock<IUserProfileService> _userProfileServiceMock { get; set; }
        private Mock<IMailer> _mailerMock { get; set; }
        private Mock<IFileNotificationSubscriberService> _fileFileNotificationSubscriberServiceMoq { get; set; }
        private Mock<IUserRolesService> _userRolesServiceMock { get; set; }
        private Mock<IUserInRolesService> _userInRolesServiceMock { get; set ; }
        private Mock<IFileProvider> _fileProviderMock { get; set; }
        private Mock<IWebSecurity> _webSecurityMock { get; set; }
        private Mock<IViewRenderWrapper> _renderViewHelper { get; set; }
        private UsersController _userController { get; set; }

        [TestInitialize()]
        public void TestInitialize()
        {
            _userProfileServiceMock = new Mock<IUserProfileService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mailerMock = new Mock<IMailer>();
            _userRolesServiceMock = new Mock<IUserRolesService>();
            _userInRolesServiceMock = new Mock<IUserInRolesService>();
            _renderViewHelper=new Mock<IViewRenderWrapper>();
            _webSecurityMock=new Mock<IWebSecurity>();
            _fileProviderMock=new Mock<IFileProvider>();
            _fileFileNotificationSubscriberServiceMoq = new Mock<IFileNotificationSubscriberService>();
            _userController = new UsersController(_unitOfWorkMock.Object, _userProfileServiceMock.Object,
                _userRolesServiceMock.Object,
                _userInRolesServiceMock.Object, _mailerMock.Object, _fileProviderMock.Object, _webSecurityMock.Object, _renderViewHelper.Object);
            _userController.SetMockControllerContext(MVC.Admin.Users.Name);
        }

        [TestMethod]
        public void ViewUsers_Test_Result()
        {
            ActionResult actionResult = _userController.ViewUsers();
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be ViewResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }


        [TestMethod]
        public void EditUser_Test_Result()
        {
            _userProfileServiceMock.Setup(u => u.GetUserById(1)).Returns(new UserProfile()
            {
                Roles = new List<UserInRoles>(),
                UserEmail = "test@test.su",
                UserId = 1,
                UserName = "test"
            });
            ActionResult actionResult = _userController.EditUser(1);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(
                    string.Format(
                        "incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                        actionResult.GetType()));
            }
            else if (!(result.Model is EditUserViewModel))
                Assert.Fail("incorrect type of model.");
        }

        [TestMethod]
        public void EditUser_Post_Model_Is_Not_Valid_Test_Result()
        {
            _userController.ModelState.AddModelError(string.Empty, string.Empty);
            var userModel = new EditUserViewModel();
            ActionResult actionResult = _userController.EditUser(userModel);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(
                    string.Format(
                        "incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                        actionResult.GetType()));
            }
            else if (!(result.Model is EditUserViewModel))
                Assert.Fail("incorrect type of model.");
        }

        [TestMethod]
        public void EditUser_Post_Model_Is_Valid_Test_Result()
        {
            _userProfileServiceMock.Setup(u => u.GetUserById(1)).Returns(new UserProfile()
            {
                Roles = new List<UserInRoles>(),
                UserEmail = "test@test.su",
                UserId = 1,
                UserName = "test"
            });
            var userModel = new EditUserViewModel()
            {
                UserId = 1,
                UserName = "test",
                UserEmail = "test@test.su"
            };
            ActionResult actionResult = _userController.EditUser(userModel);
            Assert.IsInstanceOfType(actionResult, typeof (RedirectToRouteResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be RedirectToRouteResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }

        [TestMethod]
        public void ChangeUserPassword_Test_Result()
        {
            _userProfileServiceMock.Setup(u => u.GetUserById(1)).Returns(new UserProfile()
            {
                Roles = new List<UserInRoles>(),
                UserEmail = "test@test.su",
                UserId = 1,
                UserName = "test"
            });
            _webSecurityMock.Setup(s => s.GeneratePasswordResetToken("test", It.IsAny<int>())).Returns("token");
            ActionResult actionResult = _userController.ChangeUserPassword(1);
            Assert.IsInstanceOfType(actionResult, typeof(JsonResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be JsonResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }

        [TestMethod]
        public void ViewUserDataTableFilter_Test_Result()
        {
            ActionResult actionResult = _userController.ViewUserDataTableFilter(new DefaultDataTablesRequest());
            Assert.IsInstanceOfType(actionResult, typeof (JsonResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be JsonResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }
    }
}