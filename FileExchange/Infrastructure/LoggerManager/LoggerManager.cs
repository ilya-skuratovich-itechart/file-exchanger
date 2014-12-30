using System;
using System.Text;
using NLog;

namespace FileExchange.Infrastructure.LoggerManager
{
    public class LoggerManager : ILogger
    {
        private readonly Logger _logger;

        public LoggerManager()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception x)
        {
            _logger.ErrorException(message, x);
        }

        public void Error(Exception x)
        {
            Error(BuildExceptionMessage(x));
        }

        private string BuildExceptionMessage(Exception x)
        {
            Exception logException = x;
            StringBuilder strErrorMsg = new StringBuilder();
            strErrorMsg.AppendLine("Error in Path :" + System.Web.HttpContext.Current.Request.Path);
            strErrorMsg.AppendLine("Raw Url :" + System.Web.HttpContext.Current.Request.RawUrl);
            strErrorMsg.AppendLine("----------QueryString------------:");
            foreach (string param in System.Web.HttpContext.Current.Request.QueryString)
            {
                strErrorMsg.AppendLine(string.Format("paramName: {0}. Value: {1}", param,
                    System.Web.HttpContext.Current.Request.QueryString[param]));
            }
            strErrorMsg.AppendLine("----------Form parameters------------:");
            foreach (string param in System.Web.HttpContext.Current.Request.Form)
            {
                strErrorMsg.AppendLine(string.Format("paramName: {0}. Value: {1}", param,
                    System.Web.HttpContext.Current.Request.Form[param]));
            }
            strErrorMsg.AppendLine("Message :" + logException.Message);
            strErrorMsg.AppendLine("Source :" + logException.Source);
            strErrorMsg.AppendLine("Stack Trace :" + logException.StackTrace);
            strErrorMsg.AppendLine("TargetSite :" + logException.TargetSite);
            if (logException.InnerException != null)
            {

                strErrorMsg.AppendLine("----------Inner exception---------- :");
                strErrorMsg.AppendLine("Message :" + logException.InnerException.Message);
                strErrorMsg.AppendLine("Source :" + logException.InnerException.Source);
                strErrorMsg.AppendLine("Stack Trace :" + logException.InnerException.StackTrace);
                strErrorMsg.AppendLine("TargetSite :" + logException.InnerException.TargetSite);
            }
            return strErrorMsg.ToString();
        }
    }
}