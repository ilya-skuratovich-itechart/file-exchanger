using System.Web.Mvc;
using FileExchange.Areas.Admin.Controllers;
using FileExchange.Areas.Admin.Models;
using FileExchange.Core.DAL.Entity;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.Web.UnitTests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FileExchange.Web.UnitTests.Tests.Areas.Admin
{
    [TestClass]
    public class GlobalSettingsControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock { get; set; }
        private Mock<IGlobalSettingService> _globalSettingServiceMock { get; set; }
        private GlobalSettingsController _globalSettingsController { get; set; }

        [TestInitialize()]
        public void TestInitialize()
        {
            _globalSettingServiceMock =new Mock<IGlobalSettingService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _globalSettingsController = new GlobalSettingsController(_unitOfWorkMock.Object, _globalSettingServiceMock.Object);
            _globalSettingsController.SetMockControllerContext(MVC.Admin.GlobalSettings.Name);
        }

        [TestMethod]
        public void ViewSettings_Result_Test()
        {
            ActionResult actionResult = _globalSettingsController.ViewSettings();
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be ActionResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }


        [TestMethod]
        public void UpdateSetting_Result_Test()
        {
            _globalSettingServiceMock.Setup(s => s.GetById(1)).Returns(new GlobalSetting());
            ActionResult actionResult = _globalSettingsController.UpdateSetting(1);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(
                    string.Format(
                        "incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                        actionResult.GetType()));
            }
            else if (!(result.Model is GlobalSettingViewModel))
                Assert.Fail("incorrect type of model.");
        }

        [TestMethod]
        public void UpdateSetting_Post_Model_Is_Not_Valid_Result_Test()
        {
            _globalSettingServiceMock.Setup(s => s.GetById(1)).Returns(new GlobalSetting()
            {
                VaidationRegexMask = ".*"
            });
            GlobalSettingViewModel globalSettingViewModel=new GlobalSettingViewModel()
            {
                SettingId = 1,
                SettingValue = string.Empty
            };
            _globalSettingsController.ModelState.AddModelError(string.Empty,string.Empty);
            ActionResult actionResult = _globalSettingsController.UpdateSetting(globalSettingViewModel);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(
                    string.Format(
                        "incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                        actionResult.GetType()));
            }
            else if (!(result.Model is GlobalSettingViewModel))
                Assert.Fail("incorrect type of model.");
        }

        [TestMethod]
        public void UpdateSetting_Post_Model_Is_Valid_Result_Test()
        {
            _globalSettingServiceMock.Setup(s => s.GetById(1)).Returns(new GlobalSetting()
            {
                VaidationRegexMask = ".*"
            });
            GlobalSettingViewModel globalSettingViewModel = new GlobalSettingViewModel()
            {
                SettingId = 1,
                SettingValue = string.Empty
            };
            ActionResult actionResult = _globalSettingsController.UpdateSetting(globalSettingViewModel);
            ViewResult result = actionResult as ViewResult;
            if (!_globalSettingsController.ModelState.IsValid)
                Assert.Fail("model is not valid");
            Assert.IsInstanceOfType(actionResult, typeof (RedirectToRouteResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be RedirectToRouteResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }

    }
}