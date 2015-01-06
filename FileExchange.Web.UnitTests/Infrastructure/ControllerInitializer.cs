using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using FileExchange.Infrastructure.Configuration;
using Moq;
using UrlHelper = System.Web.Mvc.UrlHelper;

namespace FileExchange.Web.UnitTests.Infrastructure
{
    public static class ControllerInitializer
    {
        private const string _host = "http://fileexchange.loc/";
        public static HttpContextBase FakeHttpContext()
        {

            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var iprincipal = new Mock<IPrincipal>();
            iprincipal.Setup(u => u.Identity.IsAuthenticated).Returns(true);
            iprincipal.Setup(u => u.Identity.Name).Returns("test");
            server.Setup(srv => srv.MapPath(It.IsAny<string>())).Returns((String a) => a.Replace("~/", @"C:\temp\").Replace("/", @"\"));
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            request.Setup(r => r.Url).Returns(new Uri(_host));
            request.Setup(r => r.Headers).Returns(new NameValueCollection());
            context.Setup(ctx => ctx.User).Returns(iprincipal.Object);
            return context.Object;
        }

        public static HttpContextBase FakeHttpContext(string url)
        {
            var context = FakeHttpContext();
            context.Request.SetFakeUrl(url);
            return context;
        }

        public static void SetFakeUrl(
            this HttpRequestBase request, string url)
        {
            Mock.Get(request.Url).Setup(h => h).Returns(new Uri(_host + url));
        }

        public static void SetFakeHttpMethod(
            this HttpRequest request, string httpMethod)
        {
            Mock.Get(request.HttpMethod).Setup(h => h).Returns(httpMethod);
        }

        public static void SetMockControllerContext(this Controller controller,
            string controllerName,
            HttpContextBase httpContext = null,
            RouteData routeData = null,
            RouteCollection routes = null)
        {
            routeData = routeData ?? new RouteData();
     
            routeData.Values.Add("controller", controllerName+"Controller");
            routes = routes ?? RouteTable.Routes;
            httpContext = httpContext ?? FakeHttpContext(); 
            var requestContext = new RequestContext(httpContext, routeData);
            var context = new ControllerContext(requestContext, controller);
            controller.Url = new UrlHelper(requestContext, routes);
            controller.ControllerContext = context;
        }

        public static void SetMockApiControllerContext(this ApiController controller)
        {
            Mock<HttpConfiguration> configurationMock = new Mock<HttpConfiguration>();
            Mock<IHttpRouteData> routeData = new Mock<IHttpRouteData>();
            Mock<HttpRequestMessage> requestMock = new Mock<HttpRequestMessage>();
            requestMock.Object.RequestUri=new Uri(_host);
            controller.Url = new System.Web.Http.Routing.UrlHelper(requestMock.Object);
            controller.ControllerContext = new HttpControllerContext(configurationMock.Object, routeData.Object,
                requestMock.Object);
         
        }
    }
}
