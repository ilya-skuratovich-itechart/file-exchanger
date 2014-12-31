using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace FileExchange.Web.UnitTests
{
    public static class HttpContextHelper
    {
        public static HttpContextBase FakeHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            context.Setup(ctx => ctx.Request.MapPath(It.IsAny<string>())).Returns("d:\test");
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.SetupProperty(ctx => ctx.User);

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
            Mock.Get(request.Url).Setup(h => h).Returns(new Uri("http://fileexchange.loc/" + url));
        }

        public static void SetFakeHttpMethod(
            this HttpRequest request, string httpMethod)
        {
            Mock.Get(request.HttpMethod).Setup(h => h).Returns(httpMethod);
        }

        public static void SetMockControllerContext(this Controller controller,
            HttpContextBase httpContext = null,
            RouteData routeData = null,
            RouteCollection routes = null)
        {
            routeData = routeData ?? new RouteData();
            routes = routes ?? RouteTable.Routes;
            httpContext = httpContext ?? FakeHttpContext();

            var requestContext = new RequestContext(httpContext, routeData);
            var context = new ControllerContext(requestContext, controller);
            controller.Url = new UrlHelper(requestContext, routes);
            controller.ControllerContext = context;
        }
    }
}
