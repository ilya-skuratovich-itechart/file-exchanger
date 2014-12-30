using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Properties;
using Autofac;
using FileExchange.Infrastructure.LoggerManager;


namespace FileExchange.Infrastructure.CustomAttirbutes
{
    public class WebApiGlobalExceptionFilter : ExceptionFilterAttribute
    {
        private ILogger _logger { get;set; }
        public WebApiGlobalExceptionFilter()
        {
            _logger=AutofacConfig.ApplicationContainer.Container.Resolve<ILogger>();
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            _logger.Error(context.Exception);
            var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("internal server error")
            };
            context.Response = resp;
        }
    }
}