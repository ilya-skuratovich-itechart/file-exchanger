using System.Collections.Generic;
using FileExchange.Core.DTO;

namespace FileExchange.Notifications.FileNotification
{
    public class FileNotificationModel
    {
        public string OriginalFileName { get; set; }

        public int FileId { get; set; }

        public string FileUrl { get; set; }

        public List<FileUserNotification> FileUserNotifications { get; set ; }
    }
}