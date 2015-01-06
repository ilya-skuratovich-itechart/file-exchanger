using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FileExchange.Controllers;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.Infrastructure.Captcha;
using FileExchange.Infrastructure.FileHelpers;
using FileExchange.Infrastructure.PageList;
using FileExchange.Infrastructure.UserSecurity;
using FileExchange.Models;
using FileExchange.Web.UnitTests.Infrastructure;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FileExchange.Web.UnitTests.Tests
{
    [TestClass]
    public class NewsControlleTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock { get; set; }

        private Mock<INewsService> _newsServiceMock { get; set; }
        private NewsController _newsController { get; set; }
        private Mock<IWebSecurity> _webSecurityMock { get; set; }
        private Mock<IFileProvider> _fileProvider { get; set; }
     

        [TestInitialize()]
        public void TestInitialize()
        {
            FileExchange.Web.UnitTests.Mappers.MapperService.RegisterMap();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _newsServiceMock = new Mock<INewsService>();
            _webSecurityMock = new Mock<IWebSecurity>();
            _fileProvider=new Mock<IFileProvider>();
          _newsController=new NewsController(_unitOfWorkMock.Object,_newsServiceMock.Object,_webSecurityMock.Object,_fileProvider.Object);
          _newsController.SetMockControllerContext(MVC.News.Name);

        }

        [TestMethod]
        public void GetLastNews_Result_Test()
        {
            _newsServiceMock.Setup(n => n.GetLastNews(It.IsAny<int>())).Returns(new List<News>()
            {
                new News()
                {
                    UniqImageName = "test",
                    Header = "test",
                    OrigImageName = "test",
                    Text = "test",
                    NewsId = 1
                }
            });
            ActionResult actionResult = _newsController.GetLastNews();
            PartialViewResult result = actionResult as PartialViewResult;
            if (result == null)
            {
                Assert.Fail(
                    string.Format(
                        "incorrect type for ActionResult. The action result should be PartialViewResult. Current actionResult has {0} type",
                        actionResult.GetType()));
            }
            else if (!(result.Model is LastNewsModel))
            {
                Assert.Fail(string.Format("incorrect type for result model. The model type should be {0} type.",
                    typeof(List<FileExchange.Models.LastNewsModel>)));
            }
            else if (((LastNewsModel)result.Model).News.Count != 1)
                Assert.Fail("incorrect count news items. the count items should be 1.");
        }

        [TestMethod]
        public void AddNews_Result_Test()
        {
            ActionResult actionResult = _newsController.AddNews();
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be ViewResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }

        [TestMethod]
        public void AddNews_Post_Model_Is_Not_Valid_Result_Test()
        {
            _newsController.ModelState.AddModelError(string.Empty, string.Empty);
            var newsModel = new CreateNewsModel();
            ActionResult actionResult = _newsController.AddNews(newsModel);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(
                    string.Format(
                        "incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                        actionResult.GetType()));
                return;
            }
            if (_newsController.ModelState.IsValid)
                Assert.Fail("Model should have errors");
            Assert.AreEqual(newsModel, result.Model, "incorrect result model.");
        }

        [TestMethod]
        public void AddNews_Post_Model_Is_Valid_Result_Test()
        {
            var newsModel = new CreateNewsModel()
            {
                File = new Mock<HttpPostedFileBase>().Object,
                Header = "test",
                ImagePath = "test",
                Text = "test"
            };
            ActionResult actionResult = _newsController.AddNews(newsModel);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be RedirectToRouteResult. Current actionResult has {0} type",
                    actionResult.GetType()));
        }

        [TestMethod]
        public void ViewNews_Result_Test()
        {
            int count;
            _newsServiceMock.Setup(n => n.GetPaged(It.IsAny<int>(), It.IsAny<int>(), out count)).Returns(
                new List<News>()
                {
                    new News()
                    {
                        Header = "test",
                        UniqImageName = "test",
                        OrigImageName = "test",
                        Text = "test"
                    }
                });
            ActionResult actionResult = _newsController.ViewNews();
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(
                    string.Format(
                        "incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                        actionResult.GetType()));
            }
            else if (!(result.Model is PagedList<ViewNewsViewModel>))
            {
                Assert.Fail(string.Format("incorrect type for result model. The model type should be {0} type.",
                    typeof (PagedList<ViewNewsViewModel>)));
            }
            else if (((PagedList<ViewNewsViewModel>) result.Model).Count != 1)
                Assert.Fail("incorrect count news items. the count items should be 1.");

        }

        [TestMethod]
        public void News_Result_Test()
        {
            _newsServiceMock.Setup(n => n.GetById(1)).Returns(new News()
            {
                Header = "test",
                UniqImageName = "test",
                OrigImageName = "test",
                Text = "test",
                NewsId = 1
            });
            ActionResult actionResult = _newsController.News(1);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(
                    string.Format(
                        "incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                        actionResult.GetType()));
            }
            else if (!(result.Model is ViewNewsViewModel))
            {
                Assert.Fail(string.Format("incorrect type for result model. The model type should be {0} type.",
                    typeof(ViewNewsViewModel)));
            }
        }

        [TestMethod]
        public void EditNews_Result_Test()
        {
            _newsServiceMock.Setup(n => n.GetById(1)).Returns(new News()
            {
                Header = "test",
                UniqImageName = "test",
                OrigImageName = "test",
                Text = "test",
                NewsId = 1
            });
            ActionResult actionResult = _newsController.EditNews(1);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(
                    string.Format(
                        "incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                        actionResult.GetType()));
            }
            else if (!(result.Model is EditNewsModel))
            {
                Assert.Fail(string.Format("incorrect type for result model. The model type should be {0} type.",
                    typeof(EditNewsModel)));
            }
        }


        [TestMethod]
        public void EditNews_Post_Model_Is_Valid_Result_Test()
        {
            Mock<HttpPostedFileBase> file=new Mock<HttpPostedFileBase>();
            file.Setup(f => f.ContentLength).Returns(1);
            EditNewsModel model=new EditNewsModel()
            {
                File = file.Object,
                Header = "test",
                ImagePath = "test",
                Text = "test",
                NewsId = 1
            };
            _newsServiceMock.Setup(n => n.GetById(1)).Returns(new News()
            {
                Header = "test",
                UniqImageName = "test",
                OrigImageName = "test",
                Text = "test",
                NewsId = 1
            });
            ActionResult actionResult = _newsController.EditNews(model);

            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be RedirectToRouteResult. Current actionResult has {0} type",
                    actionResult.GetType()));

            if (!_newsController.ModelState.IsValid)
                Assert.Fail("Model should hav't errors");
        }

        [TestMethod]
        public void EditNews_Post_Model_Is_Not_Valid_Result_Test()
        {
            Mock<HttpPostedFileBase> file = new Mock<HttpPostedFileBase>();
            file.Setup(f => f.ContentLength).Returns(1);
            _newsController.ModelState.AddModelError(string.Empty, string.Empty);
            EditNewsModel model = new EditNewsModel()
            {
                File = file.Object,
                Header = "test",
                ImagePath = "test",
                Text = "test",
                NewsId = 1
            };
            ActionResult actionResult = _newsController.EditNews(model);
            ViewResult result = actionResult as ViewResult;
            if (result == null)
            {
                Assert.Fail(
                    string.Format(
                        "incorrect type for ActionResult. The action result should be ViewResult. Current actionResult has {0} type",
                        actionResult.GetType()));
                return;
            }
            if (_newsController.ModelState.IsValid)
                Assert.Fail("Model should have errors");
            Assert.AreEqual(model, result.Model, "incorrect result model.");
        }
    }
}