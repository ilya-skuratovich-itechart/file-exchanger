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

namespace FileExchange.Controllers
{
    public partial class CaptchaController : Controller
    {
        private const int height = 30;
        private const int width = 80;
        private const int length = 4;
        private const string chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

       
        public virtual ActionResult Show()
        {
            var randomText = GenerateRandomText(length);
            var hash = ComputeMd5Hash(randomText + GetSalt());
            System.Web.HttpContext.Current.Session["CaptchaHash"] = hash;

            var rnd = new Random();
            var fonts = new[] { "Verdana", "Times New Roman" };
            float orientationAngle = rnd.Next(0, 359);

            var index0 = rnd.Next(0, fonts.Length);
            var familyName = fonts[index0];

            using (var bmpOut = new Bitmap(width, height))
            {
                var g = Graphics.FromImage(bmpOut);
                var gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, width, height),
                                                            Color.White, Color.DarkGray,
                                                            orientationAngle);
                g.FillRectangle(gradientBrush, 0, 0, width, height);
                DrawRandomLines(ref g, width, height);
                g.DrawString(randomText, new Font(familyName, 18), new SolidBrush(Color.Gray), 0, 2);
                var ms = new MemoryStream();
                bmpOut.Save(ms, ImageFormat.Png);
                var bmpBytes = ms.GetBuffer();
                bmpOut.Dispose();
                ms.Close();

                return new FileContentResult(bmpBytes, "image/png");
            }
        }

        public static bool IsValidCaptchaValue(string captchaValue)
        {
            var expectedHash = System.Web.HttpContext.Current.Session["CaptchaHash"];
            if (expectedHash == null)
                return false;
            var toCheck = captchaValue + GetSalt();
            var hash = ComputeMd5Hash(toCheck);
            System.Web.HttpContext.Current.Session["CaptchaHash"] = null;
            return hash.Equals(expectedHash);
        }

        private static void DrawRandomLines(ref Graphics g, int width, int height)
        {
            var rnd = new Random();
            var pen = new Pen(Color.Gray);
            for (var i = 0; i < 10; i++)
            {
                g.DrawLine(pen, rnd.Next(0, width), rnd.Next(0, height),
                                rnd.Next(0, width), rnd.Next(0, height));
            }
        }

        private static string GetSalt()
        {
            return typeof(CaptchaController).Assembly.FullName;
        }

        private static string ComputeMd5Hash(string input)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(input);
            HashAlgorithm md5Hasher = MD5.Create();
            return BitConverter.ToString(md5Hasher.ComputeHash(bytes));
        }

        private static string GenerateRandomText(int textLength)
        {
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, textLength)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }

    }
}
