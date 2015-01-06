using System.Web.Mvc;
using System.Web.UI.WebControls;
using FileExchange.Controllers;
using FileExchange.Core.BandwidthThrottling;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.EmailSender;
using FileExchange.Infrastructure.Captcha;
using FileExchange.Infrastructure.FileHelpers;
using FileExchange.Infrastructure.UserSecurity;
using FileExchange.Infrastructure.ViewsWrappers;
using FileExchange.Models;
using FileExchange.Web.UnitTests.Infrastructure;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FileExchange.Web.UnitTests.Tests
{
    [TestClass]
    public class AccountControllerTests
    {

        private Mock<IUnitOfWork> _unitOfWorkMock { get; set; }
      
        private Mock<IUserProfileService> _userProfileServiceMock { get; set; }
        private AccountController _accountController { get; set; }
        private Mock<IWebSecurity> _webSecurityMock { get; set; }
        private Mock<IUserInRolesService> _userInRolesServiceMock { get; set; }
        private Mock<ICaptcha> _captchaHelperMock { get; set; }
        private CompareLogic _compareLogic { get; set; }

        private const int CurrentUserId = 1;

        [TestInitialize()]
        public void TestInitialize()
        {
            FileExchange.Web.UnitTests.Mappers.MapperService.RegisterMap();
            _compareLogic = new CompareLogic();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userInRolesServiceMock = new Mock<IUserInRolesService>();
            _userProfileServiceMock = new Mock<IUserProfileService>();
            _webSecurityMock = new Mock<IWebSecurity>();
            _captchaHelperMock = new Mock<ICaptcha>();
            _userProfileServiceMock.Setup(u => u.GetUserById(It.IsAny<int>())).Returns((int userId) => new UserProfile()
            {
                UserId = userId,
                MaxDonwloadSpeedKbps = 0,
                FileMaxSizeKbps = 0,
                UserEmail = "test@test.su",
                UserName = "Test"
            });
            _accountController = new AccountController(_unitOfWorkMock.Object, _userProfileServiceMock.Object,
                _userInRolesServiceMock.Object, _webSecurityMock.Object, _captchaHelperMock.Object);
            _accountController.SetMockControllerContext(MVC.Account.Name);

        }

        [TestMethod]
        public void Login_Result_Test()
        {
            ActionResult actionResult = _accountController.Login(string.Empty);
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be ViewResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }

        [TestMethod]
        public void Login_Post_Model_Is_Not_Valid_Result_Test()
        {
            var loginModel = new LoginModel();
            ActionResult actionResult = _accountController.Login(loginModel,string.Empty);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
            if (_accountController.ModelState.IsValid)
                Assert.Fail("Model should have errors");
        }

         [TestMethod]
        public void Login_Post_Model_Is_Valid_Result_Test()
         {
             _webSecurityMock.Setup(s => s.Login("test", "1234567",It.IsAny<bool>())).Returns(true);
            var loginModel = new LoginModel()
            {
                Password = "1234567",
                UserName = "test"
            };
            ActionResult actionResult = _accountController.Login(loginModel, string.Empty);
            RedirectToRouteResult result = actionResult as RedirectToRouteResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. The action result should be RedirectToRouteResult. Current actionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
            if (!_accountController.ModelState.IsValid)
                Assert.Fail("Model should hav't errors");
        }

         [TestMethod]
         public void Login_LogOff_Result_Test()
         {
            ActionResult actionResult = _accountController.LogOff();
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be RedirectToRouteResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }

         [TestMethod]
         public void Login_UserBanned_Result_Test()
         {
             ActionResult actionResult = _accountController.UserBanned();
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be ActionResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }

         [TestMethod]
         public void Login_Register_Result_Test()
         {
             ActionResult actionResult = _accountController.Register();
             Assert.IsInstanceOfType(actionResult, typeof(ActionResult),
                 string.Format(
                     "incorrect type for ActionResult.The action result should be ActionResult. Current actionResult has {0} type",
                     actionResult.GetType()));
         }

         [TestMethod]
         public void Login_Register_Test()
         {
             ActionResult actionResult = _accountController.Register();
             Assert.IsInstanceOfType(actionResult, typeof(ViewResult),
                 string.Format(
                     "incorrect type for ActionResult.The action result should be ViewResult. Current actionResult has {0} type",
                     actionResult.GetType()));
         }

         [TestMethod]
         public void Register_Post_Model_Is_Not_Valid_Result_Test()
         {
             _captchaHelperMock.Setup(c => c.IsValidCaptchaValue(It.IsAny<string>())).Returns(true);
             _accountController.ModelState.AddModelError(string.Empty,string.Empty);
             var registerModel = new RegisterModel();
             ActionResult actionResult = _accountController.Register(registerModel, string.Empty);
             ViewResult result = actionResult as ViewResult;
             if (result == null)
             {
                 Assert.Fail(string.Format("incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                     actionResult.GetType()));
                 return;
             }
             Assert.AreEqual(result.Model, registerModel, string.Format("incorrect type for ActionResult Model. The model type should be {0}. Current actionResult has {1} type",
                 typeof(RegisterModel),actionResult.GetType()));
             if (_accountController.ModelState.IsValid)
                 Assert.Fail("Model should have errors");
         }

         [TestMethod]
         public void Register_Post_Model_Is_Valid_Result_Test()
         {
             _captchaHelperMock.Setup(c => c.IsValidCaptchaValue(It.IsAny<string>())).Returns(true);
             _webSecurityMock.Setup(s => s.Login("test", "123456", It.IsAny<bool>())).Returns(true);
             var registerModel = new RegisterModel()
             {
                 Password = "123456",
                 ConfirmPassword = "123456",
                 Email = "test@test.su",
                 UserName = "test"
             };
             ActionResult actionResult = _accountController.Register(registerModel, string.Empty);
             RedirectToRouteResult result = actionResult as RedirectToRouteResult;
             if (result == null)
             {
                 Assert.Fail(string.Format("incorrect type for ActionResult. The action result should be RedirectToRouteResult. Current actionResult has {0} type",
                     actionResult.GetType()));
                 return;
             }
             if (!_accountController.ModelState.IsValid)
                 Assert.Fail("Model should hav't errors");
         }

        

        
    }
}