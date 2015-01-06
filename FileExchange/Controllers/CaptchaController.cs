using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FileExchange.Infrastructure.Captcha;

namespace FileExchange.Controllers
{
    public partial class CaptchaController : Controller
    {
        private ICaptcha _captchaHelper { get; set; }

        public CaptchaController(ICaptcha captchaHelper)
        {
            _captchaHelper = captchaHelper;
        }

        public virtual ActionResult Show()
        {
            return new FileContentResult(_captchaHelper.GetCaptchaImage(), "image/png");
        }
    }
}
