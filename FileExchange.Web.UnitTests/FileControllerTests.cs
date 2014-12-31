using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using FileExchange.Controllers;
using FileExchange.Core.BandwidthThrottling;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.EmailSender;
using FileExchange.Infrastructure.FileHelpers;
using FileExchange.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FileExchange.Web.UnitTests
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
        private Mock<IUserProfileService> _userProfileService { get; set; }
        private Mock<IMailer> _mailerMock { get; set; }
        private FileController _fileController { get; set; }
        private Mock<IFileProvider> _fileProviderMock { get; set; }


        [TestInitialize()]
        public void TestInitialize()
        {
            FileExchange.Web.UnitTests.Mappers.MapperService.RegisterMap();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _fileFileNotificationSubscriberServiceMoq = new Mock<IFileNotificationSubscriberService>();
            _fileCategoriesServiceMock = new Mock<IFileCategoriesService>();
            _fileExchangeServiceMock = new Mock<IExchangeFileService>();
            _fileCommentServiceMock = new Mock<IFileCommentService>();
            _bandwidthThrottlingSettingsMock = new Mock<IBandwidthThrottlingSettings>();
            _userProfileService = new Mock<IUserProfileService>();
            _mailerMock = new Mock<IMailer>();
            _fileProviderMock=new Mock<IFileProvider>();

            _fileController = new FileController(_unitOfWorkMock.Object, _fileCategoriesServiceMock.Object,
                _fileFileNotificationSubscriberServiceMoq.Object,
                _fileExchangeServiceMock.Object, _fileCommentServiceMock.Object, _bandwidthThrottlingSettingsMock.Object,
                _userProfileService.Object, _mailerMock.Object, _fileProviderMock.Object);
            _fileController.SetMockControllerContext();
        }

        [TestMethod]
        public void EditUser_Model_Test()
        {
            _fileCategoriesServiceMock.Setup(f => f.GetAll()).Returns(new List<FileCategories>()
            {
                new FileCategories()
                {
                    CategoryId = 1,
                    CategoryName = "test"
                }
            });
            List<FileCategoryModel> correctModel = AutoMapper.Mapper.Map<List<FileCategoryModel>>(_fileCategoriesServiceMock.Object.GetAll());
            PartialViewResult result = _fileController.FileSections() as PartialViewResult;
            var model = result.Model as List<FileCategoryModel>;
            AssertEx.AssertListsAreEquals(null,model, correctModel);

        }
    }
    public static class AssertEx
    {
        public static void PropertyValuesAreEquals(object actual, object expected)
        {
            PropertyInfo[] properties = expected.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object expectedValue = property.GetValue(expected, null);
                object actualValue = property.GetValue(actual, null);

                if (actualValue is IList)
                    AssertListsAreEquals(property, (IList)actualValue, (IList)expectedValue);
                else if (!Equals(expectedValue, actualValue))
                    Assert.Fail("Property {0}.{1} does not match. Expected: {2} but was: {3}", property.DeclaringType.Name, property.Name, expectedValue, actualValue);
            }
        }

        private static void AssertListsAreEquals(PropertyInfo property, IList actualList, IList expectedList)
        {
            if (actualList.Count != expectedList.Count)
                Assert.Fail("Property {0}.{1} does not match. Expected IList containing {2} elements but was IList containing {3} elements", property.PropertyType.Name, property.Name, expectedList.Count, actualList.Count);

            for (int i = 0; i < actualList.Count; i++)
                if (!Equals(actualList[i], expectedList[i]))
                    Assert.Fail("Property {0}.{1} does not match. Expected IList with element {1} equals to {2} but was IList with element {1} equals to {3}", property.PropertyType.Name, property.Name, expectedList[i], actualList[i]);
        }
    }
}
