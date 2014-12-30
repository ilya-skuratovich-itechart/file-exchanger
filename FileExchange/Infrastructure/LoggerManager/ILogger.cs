using System;

namespace FileExchange.Infrastructure.LoggerManager
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
        void Error(string message, Exception x);
        void Error(Exception x);
    }
}