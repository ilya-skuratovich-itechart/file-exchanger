using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace FileExchange.Helplers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString RemoveHtmlTags(this HtmlHelper html,string htmlContent,int? displayLength=null)
        {
            string content = Regex.Replace(htmlContent, @"<[^>]*>", String.Empty);
            if (displayLength.HasValue)
                content = content.Length > displayLength.Value ? content.Remove(displayLength.Value) : content;
            return MvcHtmlString.Create(content);
        }
    }
}