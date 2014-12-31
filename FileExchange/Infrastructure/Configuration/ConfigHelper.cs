using System.IO;
using FileExchange.Core.Migrations;

namespace FileExchange.Infrastructure.Configuration
{
    public static class ConfigHelper
    {
        public static string FilesFolder = System.Configuration.ConfigurationManager.AppSettings["uploadFileFolder"]; 
    }
}