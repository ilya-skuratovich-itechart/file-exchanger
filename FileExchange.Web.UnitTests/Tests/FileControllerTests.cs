using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using FileExchange.Controllers;
using FileExchange.Core.BandwidthThrottling;
using FileExchange.Core.DAL.Entity;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.EmailSender;
using FileExchange.Infrastructure.ActionResults;
using FileExchange.Infrastructure.Configuration;
using FileExchange.Infrastructure.FileHelpers;
using FileExchange.Infrastructure.UserSecurity;
using FileExchange.Infrastructure.ViewsHelpers;
using FileExchange.Models;
using FileExchange.Models.DataTable;
using FileExchange.Web.UnitTests.Infrastructure;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebMatrix.WebData;

namespace FileExchange.Web.UnitTests.Tests
{
    [TestClass]
    public class FileControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock { get; set; }
        private Mock<IFileNotificationSubscriberService> _fileFileNotificationSubscriberServiceMoq { get; set; }
        private Mock<IFileCategoriesService> _fileCategoriesServiceMock { get; set; }
        private Mock<IExchangeFileService> _fileExchangeServiceMock { get; set; }
        private Mock<IFileCommentService> _fileCommentServiceMock { get; set; }
        private Mock<IBandwidthThrottlingSettings> _bandwidthThrottlingSettingsMock { get; set; }
        private Mock<IUserProfileService> _userProfileServiceMock { get; set; }
        private Mock<IMailer> _mailerMock { get; set; }
        private FileController _fileController { get; set; }
        private Mock<IFileProvider> _fileProviderMock { get; set; }
        private Mock<IWebSecurity> _webSecurityMock{ get; set; }
        private CompareLogic _compareLogic { get; set; }
        private Mock<IViewRenderWrapper> _viewRenderWrapperMock { get; set; }
        private const int CurrentUserId = 1;
        [TestInitialize()]
        public void TestInitialize()
        {
            FileExchange.Web.UnitTests.Mappers.MapperService.RegisterMap();
            _compareLogic = new CompareLogic();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _fileFileNotificationSubscriberServiceMoq = new Mock<IFileNotificationSubscriberService>();
            _fileCategoriesServiceMock = new Mock<IFileCategoriesService>();
            _fileExchangeServiceMock = new Mock<IExchangeFileService>();
            _fileCommentServiceMock = new Mock<IFileCommentService>();
            _bandwidthThrottlingSettingsMock = new Mock<IBandwidthThrottlingSettings>();
            _userProfileServiceMock = new Mock<IUserProfileService>();
            _mailerMock = new Mock<IMailer>();
            _fileProviderMock = new Mock<IFileProvider>();
            _webSecurityMock = new Mock<IWebSecurity>();
            _viewRenderWrapperMock = new Mock<IViewRenderWrapper>();
            _webSecurityMock.Setup(s => s.GetCurrentUserId()).Returns(CurrentUserId);
           _userProfileServiceMock.Setup(u => u.GetUserById(It.IsAny<int>())).Returns((int userId) => new UserProfile()
            {
                UserId = userId,
                MaxDonwloadSpeedKbps = 0,
                FileMaxSizeKbps = 0,
                UserEmail = "test@test.su",
                UserName = "Test"
            });

            _fileController = new FileController(_unitOfWorkMock.Object, _fileCategoriesServiceMock.Object,
                _fileFileNotificationSubscriberServiceMoq.Object,
                _fileExchangeServiceMock.Object, _fileCommentServiceMock.Object, _bandwidthThrottlingSettingsMock.Object,
                _userProfileServiceMock.Object, _mailerMock.Object, _fileProviderMock.Object, _webSecurityMock.Object, _viewRenderWrapperMock.Object);
            _fileController.SetMockControllerContext(MVC.File.Name);
         
        }



        [TestMethod]
        public void EditUser_Result_Test()
        {
           
            _fileCategoriesServiceMock.Setup(f => f.GetAll()).Returns(new List<FileCategories>()
            {
                new FileCategories()
                {
                    CategoryId = 1,
                    CategoryName = "test"
                }
            });
            ActionResult actionResult = _fileController.FileSections();
            PartialViewResult result = actionResult as PartialViewResult;
            List<FileCategoryModel> correctViewModel =
                AutoMapper.Mapper.Map<List<FileCategoryModel>>(_fileCategoriesServiceMock.Object.GetAll());
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
            var resultViewModel = result.Model as List<FileCategoryModel>;
            Assert.IsTrue(_compareLogic.Compare(correctViewModel, resultViewModel).AreEqual);
        }

       

        [TestMethod]
        public void UserFiles_Result_Test()
        {
            ActionResult actionResult = _fileController.UserFiles();
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
            if (result.Model != null)
                Assert.Fail("Model must be null");
        }

        [TestMethod]
        public void AddUserFile_Result_Test()
        {
            _fileCategoriesServiceMock.Setup(f => f.GetAll()).Returns(new List<FileCategories>()
            {
                new FileCategories()
                {
                    CategoryId = 1,
                    CategoryName = "test"
                }
            });
            ActionResult actionResult = _fileController.AddUserFile();
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
            if (!(result.Model is CreateExchangeFileModel))
            {
                Assert.Fail(string.Format("Model must be {0} type", typeof (CreateExchangeFileModel)));
            }
            else
            {
                CreateExchangeFileModel model = result.Model as CreateExchangeFileModel;
                if (model.FileCategories.Count() != 1)
                    Assert.Fail("Count items for CreateExchangeFileModel.FileCategories items must be 1.");
            }
        }

        [TestMethod]
        public void AddUserFile_Post_Result_Test_Model_Is_Not_Valid()
        {
            _fileCategoriesServiceMock.Setup(f => f.GetAll()).Returns(new List<FileCategories>()
            {
                new FileCategories()
                {
                    CategoryId = 1,
                    CategoryName = "test"
                }
            });
          
            CreateExchangeFileModel model = new CreateExchangeFileModel();
            ActionResult actionResult = _fileController.AddUserFile(model);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
            if (!(result.Model is CreateExchangeFileModel))
            {
                Assert.Fail(string.Format("Model should be of {0} type", typeof (CreateExchangeFileModel)));
            }
            else
            {
                CreateExchangeFileModel viewModel = result.Model as CreateExchangeFileModel;
                if (viewModel.FileCategories.Count() != 1)
                    Assert.Fail("Count items for CreateExchangeFileModel.FileCategories items must be 1.");
            }
        }


        [TestMethod]
        public void AddUserFile_Post_Result_Model_Is_Valid()
        {
            CreateExchangeFileModel model = new CreateExchangeFileModel()
            {
                Description = "test",
                File = new Mock<HttpPostedFileBase>().Object,
                SelectedFileCategoryId = 1,
                Tags = "test"
            };
            ActionResult actionResult = _fileController.AddUserFile(model);
            RedirectToRouteResult result = actionResult as RedirectToRouteResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
        }

        [TestMethod]
        public void EditUserFile_Result_Test()
        {
            _fileExchangeServiceMock.Setup(f => f.GetUserFile(It.IsAny<int>(), It.IsAny<int>())).Returns(
                new ExchangeFile());
            ActionResult actionResult = _fileController.EditUserFile(1);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
            Assert.IsInstanceOfType(result.Model, typeof (EditExchangeFileModel),
                string.Format("the model should be of {0} type", typeof (EditExchangeFileModel)));

        }

        [TestMethod]
        public void EditUserFile_Post_Result_Test_Model_Is_Not_Valid()
        {
            _fileCategoriesServiceMock.Setup(f => f.GetAll()).Returns(new List<FileCategories>()
            {
                new FileCategories()
                {
                    CategoryId = 1,
                    CategoryName = "test"
                }
            });
           _fileController.ModelState.AddModelError(string.Empty,string.Empty);
          ActionResult actionResult = _fileController.EditUserFile(new EditExchangeFileModel());
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
            Assert.IsInstanceOfType(result.Model, typeof (EditExchangeFileModel),
                string.Format("the model should be of {0} type", typeof (EditExchangeFileModel)));
        }

        [TestMethod]
        public void EditUserFile_Post_Result_Test_Model_Is_Valid()
        {
            _fileCategoriesServiceMock.Setup(f => f.GetAll()).Returns(new List<FileCategories>()
            {
                new FileCategories()
                {
                    CategoryId = 1,
                    CategoryName = "test"
                }
            });

            _fileFileNotificationSubscriberServiceMoq.Setup(s => s.GetFileNotificationSubscriberses(It.IsAny<int>()))
                .Returns(new List<FileNotificationSubscribers>());
            _fileExchangeServiceMock.Setup(f => f.GetUserFile(1, It.IsAny<int>())).Returns(new ExchangeFile());
            _fileExchangeServiceMock.Setup(
                f =>
                    f.Update(CurrentUserId, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(new ExchangeFile()
                {
                    Description = "test",
                    FileId = 1,
                    OrigFileName = "test",
                    FileCategoryId = 1,
                    Tags = "test"
                });

            var userFile = new EditExchangeFileModel()
            {
                Description = "test",
                FileId = 1,
                OrigFileName = "test",
                SelectedFileCategoryId = 1,
                Tags = "test"
            };
           
            ActionResult actionResult = _fileController.EditUserFile(userFile);
            Assert.IsInstanceOfType(actionResult, typeof (RedirectToRouteResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be RedirectToRouuteResult.Current actionResult has {0} type",
                    actionResult.GetType()));
        }

        [TestMethod]
        public void DeleteUserFile_Test_Result()
        {
            const int removeFileId = 1;
            _fileExchangeServiceMock.Setup(f => f.RemoveUserFile(CurrentUserId, removeFileId));
            _fileExchangeServiceMock.Setup(f => f.GetUserFile(removeFileId, It.IsAny<int>())).Returns(new ExchangeFile()
            {
                FileId = removeFileId,
                OrigFileName = "test",
                UniqFileName = "test",
                Description = "test",
                FileCategoryId = 1
            });
            ActionResult actionResult = _fileController.DeleteUserFile(removeFileId);
            Assert.IsInstanceOfType(actionResult, typeof (ViewResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be ViewResult.Current actionResult has {0} type",
                    actionResult.GetType()));
        }

        [TestMethod]
        public void ViewCategoryFiles_Result_Test()
        {
            ActionResult actionResult = _fileController.ViewCategoryFiles(1);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
            Assert.IsInstanceOfType(result.Model, typeof(int),
                string.Format("the model should be of integer type"));
        }

        [TestMethod]
        public void ViewFile_Result_Test()
        {
            const int fileId = 1;
            _fileExchangeServiceMock.Setup(f => f.GetFilteredFile(fileId, It.IsAny<bool>())).Returns(new ExchangeFile()
            {
                FileId = fileId,
                User = new UserProfile()
            });
            _fileFileNotificationSubscriberServiceMoq.Setup(s => s.UserIsSubscibed(CurrentUserId, fileId))
                .Returns(true);
            ActionResult actionResult = _fileController.ViewFile(fileId);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type",
                    actionResult.GetType()));
                return;
            }
            Assert.IsInstanceOfType(result.Model, typeof(ViewExchangeFileViewModel), string.Format(
                    "incorrect type for Model.The modelt should be {0} type.", typeof(ViewExchangeFileViewModel)));
        }


        [TestMethod]
        public void ViewFileComments_Result_Test()
        {
            const int fileId = 1;
            _fileCommentServiceMock.Setup(f => f.GetFileComments(fileId)).Returns(new List<FileComments>());
         
            ActionResult actionResult = _fileController.ViewFileComments(fileId);
            PartialViewResult result = actionResult as PartialViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type.ActionResult should be of PartialViewResult.",
                    actionResult.GetType()));
                return;
            }
            Assert.IsInstanceOfType(result.Model, typeof(IEnumerable<FileCommentsViewModel>), string.Format(
                    "incorrect type for Model.The modelt should be {0} type.", typeof(IEnumerable<FileCommentsViewModel>)));
        }

        [TestMethod]
        public void AddComment_Result_Test()
        {
            ActionResult actionResult = _fileController.AddComment(1);
            PartialViewResult result = actionResult as PartialViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type.ActionResult should be of PartialViewResult.",
                    actionResult.GetType()));
                return;
            }
            Assert.IsInstanceOfType(result.Model, typeof(CreateCommentViewModel), string.Format(
                    "incorrect type for Model.The modelt should be {0} type.", typeof(CreateCommentViewModel)));

        }


        [TestMethod]
        public void AddComment_Post_Result_Test_Model_Is_Not_Valid()
        {
            CreateCommentViewModel commentModel = new CreateCommentViewModel()
            {
                Comment = string.Empty
            };
            _fileController.ModelState.AddModelError(string.Empty, string.Empty);
            _fileController.Request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            ActionResult actionResult = _fileController.AddComment(commentModel);
            Assert.IsInstanceOfType(actionResult, typeof (ContentResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be ContentResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }

        [TestMethod]
        public void AddComment_Post_Result_Test_Model_IsValid()
        {
            CreateCommentViewModel commentModel = new CreateCommentViewModel()
            {
                FileId = 1,
                Comment = "test"
            };
            _fileController.Request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            ActionResult actionResult = _fileController.AddComment(commentModel);
            PartialViewResult result = actionResult as PartialViewResult;
            if (result == null)
            {
                Assert.Fail(string.Format("incorrect type for ActionResult. ActionResult has {0} type.ActionResult should be of PartialViewResult.",
                    actionResult.GetType()));
                return;
            }
            Assert.IsInstanceOfType(result.Model, typeof(IEnumerable<FileCommentsViewModel>), string.Format(
                    "incorrect type for Model.The modelt should be {0} type.", typeof(IEnumerable<FileCommentsViewModel>)));
        }

        [TestMethod]
        public void DownloadFile_Result_Test()
        {
            const int fileId = 1;
            _fileExchangeServiceMock.Setup(f => f.GetFilteredFile(fileId, It.IsAny<bool>())).Returns(new ExchangeFile()
            {
                FileId = fileId,
                UniqFileName = "test",
                OrigFileName = "test"
            });

           
            ActionResult actionResult = _fileController.DownloadFile(fileId);
            Assert.IsInstanceOfType(actionResult, typeof(BandwidthThrottlingFileResult),
                 string.Format(
                     "incorrect type for ActionResult.The action result should be BandwidthThrottlingFileResult. Current actionResult has {0} type",
                     actionResult.GetType()));
        }

        [TestMethod]
        public void SubscribeFileNotification_Result_test()
        {
            const int fileId = 1;
            _fileFileNotificationSubscriberServiceMoq.Setup(s => s.UserIsSubscibed(CurrentUserId, fileId))
                .Returns(false);
            _fileFileNotificationSubscriberServiceMoq.Setup(s => s.Add(CurrentUserId, fileId)).Returns(
                new FileNotificationSubscribers()
                {
                    FileId = fileId,
                    Subscriberid = 1,
                    UserId = CurrentUserId
                });
            ActionResult actionResult = _fileController.SubscribeFileNotification(fileId);
            Assert.IsInstanceOfType(actionResult, typeof(JsonResult),
                 string.Format(
                     "incorrect type for ActionResult.The action result should be JsonResult. Current actionResult has {0} type",
                     actionResult.GetType()));
        }

        [TestMethod]
        public void UnscribeFileNotification_Result_test()
        {
            const int fileId = 1;
            _fileFileNotificationSubscriberServiceMoq.Setup(s => s.UserIsSubscibed(CurrentUserId, fileId))
                .Returns(true);
           
            ActionResult actionResult = _fileController.UnscribeFileNotification(fileId);
            Assert.IsInstanceOfType(actionResult, typeof(JsonResult),
                 string.Format(
                     "incorrect type for ActionResult.The action result should be JsonResult. Current actionResult has {0} type",
                     actionResult.GetType()));
        }
        
        [TestMethod]
        public void ViewCategoryFilesTableFilter_Result_test()
        {
            ActionResult actionResult = _fileController.ViewCategoryFilesTableFilter(new DefaultDataTablesRequest());
            Assert.IsInstanceOfType(actionResult, typeof(JsonResult),
                 string.Format(
                     "incorrect type for ActionResult.The action result should be JsonResult. Current actionResult has {0} type",
                     actionResult.GetType()));
        }

        [TestMethod]
        public void UserFilesTableFilter_Result_test()
        {
            ActionResult actionResult = _fileController.UserFilesTableFilter(new JQueryDataTablesModel());
            Assert.IsInstanceOfType(actionResult, typeof(JsonResult),
                 string.Format(
                     "incorrect type for ActionResult.The action result should be JsonResult. Current actionResult has {0} type",
                     actionResult.GetType()));
        }
    }
}
