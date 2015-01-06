using System.Web.Mvc;
using FileExchange.Controllers;
using FileExchange.Infrastructure.Captcha;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FileExchange.Web.UnitTests.Tests
{
    [TestClass]
    public class CaptchaControllerTest
    {
        private Mock<ICaptcha> _captchaMock { get; set; }
        private CaptchaController _captchaController { get; set; }

        [TestInitialize()]
        public void TestInitialize()
        {
            _captchaMock = new Mock<ICaptcha>();
            _captchaController = new CaptchaController(_captchaMock.Object);
        }

        [TestMethod]
        public void Show_Test_Result()
        {
            ActionResult result = _captchaController.Show();
            Assert.IsInstanceOfType(result, typeof (FileContentResult),
                string.Format(
                    "incorrect type for ActionResult.The action result should be FileContentResult. Current actionResult has {0} type",
                    result.GetType()));
        }
    }
}