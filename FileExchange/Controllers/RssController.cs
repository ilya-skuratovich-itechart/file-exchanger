using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;
using FileExchange.Infrastructure.CustomHttpContent;
using FileExchange.Infrastructure.Rss;

namespace FileExchange.Controllers
{
    public class RssController : ApiController
    {
        private INewsService _newsService { get; set; }

        public RssController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public HttpResponseMessage Get()
        {
            string host = string.Format("http://www.{0}",System.Web.HttpContext.Current.Request.Url.Host.ToLower());
            string newsUrl =string.Format("{0}/{1}/{2}",host,MVC.News.Name,MVC.News.ActionNames.ViewNews);

            Rss20FeedFormatter rss20FeedFormatter = RssService.GetRssNews(_newsService.GetAll(), newsUrl, host);
            var output = new StringBuilder();
            using (var writer = XmlWriter.Create(output, new XmlWriterSettings { Indent = true }))
            {
                rss20FeedFormatter.WriteTo(writer);
                writer.Flush();
               
            }
            var doc = new XmlDocument();
            doc.LoadXml(output.ToString());
            doc.LoadXml(doc.SelectSingleNode("rss").OuterXml);
            return new HttpResponseMessage()
            {
                Content = new JsonHttpContent(Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc))
            };
        }
    }
}