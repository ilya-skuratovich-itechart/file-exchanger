using System;
using FileExchange.Core.BandwidthThrottling;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileExchange.Web.UnitTests
{
    [TestClass]
    public class FileControllerTests
    {
        private IUnitOfWork _unitOfWork { get; set; }
        private IFileNotificationSubscriberService _fileFileNotificationSubscriberService { get; set; }
        private IFileCategoriesService _fileCategoriesService { get; set; }
        private IExchangeFileService _fileExchangeService { get; set; }
        private IFileCommentService _fileCommentService { get; set; }
        private IBandwidthThrottlingSettings _bandwidthThrottlingSettings { get; set; }
        private IUserProfileService _userProfileService { get; set; }
        [TestInitialize()]
        public void MyTestInitialize()
        {

        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
