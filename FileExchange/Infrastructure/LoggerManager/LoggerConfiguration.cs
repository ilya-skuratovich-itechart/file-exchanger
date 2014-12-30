using NLog;

namespace FileExchange.Infrastructure.LoggerManager
{
    public static class LoggerConfiguration
    {
        public static Logger Logger { get; private set; } 
        static LoggerConfiguration()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }
      
    }
}