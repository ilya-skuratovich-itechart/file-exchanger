﻿using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Xipton.Razor;

namespace FileExchange.Infrastructure.ViewsHelpers
{
    public class ViewRenderWrapper : IViewRenderWrapper
    {
        public string RenderViewToString(Controller controller, string viewName, object model)
        {
            using (var writer = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                controller.ViewData.Model = model;
                var viewCxt = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer);
                viewCxt.View.Render(viewCxt, writer);
                return writer.ToString();
            }
        }

        public string RenderViewToString(string viewPath, string layoutPath, object model)
        {

            string content = File.ReadAllText(MapPath(viewPath));
            string layout = File.ReadAllText(MapPath(layoutPath));
            var rm = new RazorMachine();
            rm.RegisterTemplate("~/shared/_layout.cshtml", layout);

            var renderedContent =
                rm.ExecuteContent(string.Format("{0}{1}", "@{Layout=\"_layout\";}", content), model);
            var result = renderedContent.Result;
            return result;
        }

        private string MapPath(string filePath)
        {
            return HttpContext.Current != null
                ? HttpContext.Current.Server.MapPath(filePath)
                : string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory,
                    filePath.Replace("~", string.Empty).TrimStart('/'));
        }
    }
}