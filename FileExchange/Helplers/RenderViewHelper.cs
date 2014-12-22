﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using RazorTemplates.Core;
using Xipton.Razor;

namespace FileExchange.Helplers
{
    public static class RenderViewHelper
    {
        private static Dictionary<string, dynamic> _templatesCache = new Dictionary<string, dynamic>();
     
        public  static string RenderPartialToString(string viewPath, object model)
        {
            var rm = new RazorMachine();

            if (!_templatesCache.ContainsKey(viewPath))
            {
                string content = File.ReadAllText(MapPath(viewPath));
                var compiledTemplate = Template.Compile(content);
                _templatesCache.Add(viewPath, compiledTemplate);
            }

            return  _templatesCache[viewPath].Render(model);
        }

        private static RazorMachine CreateRazorMachineWithoutContentProviders(bool includeGeneratedSourceCode = false, string rootOperatorPath = null, bool htmlEncode = true)
        {
            var rm = new RazorMachine(includeGeneratedSourceCode: includeGeneratedSourceCode, htmlEncode: htmlEncode, rootOperatorPath: rootOperatorPath);
            rm.Context.TemplateFactory.ContentManager.ClearAllContentProviders();
            return rm;
        }

        public static string MapPath(string filePath)
        {
            return HttpContext.Current != null ? HttpContext.Current.Server.MapPath(filePath) : string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, filePath.Replace("~", string.Empty).TrimStart('/'));
        } 
    }
}