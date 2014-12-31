﻿using System.Collections.Generic;
using FileExchange.Core.DTO;
using FileExchange.EmailSender;

namespace FileExchange.Infrastructure.Notifications.FileNotification
{
    public class FileNotificationModel
    {
        public IMailer Mailer { get; set; }

        public string OriginalFileName { get; set; }

        public int FileId { get; set; }

        public string FileUrl { get; set; }

        public List<FileUserNotification> FileUserNotifications { get; set ; }
    }
}