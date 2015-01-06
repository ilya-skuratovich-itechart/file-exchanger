using System;
using System.Collections.Generic;
using System.Net.Http;
using FileExchange.Controllers;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;
using FileExchange.Infrastructure.CustomHttpContent;
using FileExchange.Web.UnitTests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FileExchange.Web.UnitTests.Tests
{
    [TestClass]
    public class RssControllerTest
    {
        private Mock<INewsService> _newsServiceMock { get; set; } 
        private RssController _rssController;
        [TestInitialize]
        public void InitTest()
        {
            _newsServiceMock=new Mock<INewsService>();
            _rssController = new RssController(_newsServiceMock.Object);
            _rssController.SetMockApiControllerContext();
        }

        [TestMethod]
        public void Get_Result_Test()
        {
            _newsServiceMock.Setup(n => n.GetAll()).Returns(new List<News>()
            {
                new News()
                {
                    CreateDate = DateTime.Now,
                    Header = "test",
                    NewsId = 1,
                    OrigImageName = "test",
                    Text = "test",
                    UniqImageName = "test"
                }
            });
            HttpResponseMessage message = _rssController.Get();
            Assert.AreNotEqual(message.StatusCode, 200,
                string.Format("incorrect status code. Current code is {0}", message.StatusCode));
            Assert.IsInstanceOfType(message.Content, typeof (JsonHttpContent),
                "incorrect type for response content. Content should have {0} type.", typeof (JsonHttpContent));
        }
    }
}