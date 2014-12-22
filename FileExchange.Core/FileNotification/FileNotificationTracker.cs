using System.Collections.Generic;
using System.Linq;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.DTO;
using FileExchange.Core.Services;

namespace FileExchange.Core.FileNotification
{
    public class FileNotificationTracker
    {
        private IFileNotificationSubscriberService _fileNotificationSubscriberService { get; set; }
        public FileNotificationTracker(IFileNotificationSubscriberService fileNotificationSubscriberService)
        {
            _fileNotificationSubscriberService = fileNotificationSubscriberService;
        }

        /// <summary>
        /// if file is deleted, newExchangeFile must be a null
        /// </summary>
        /// <param name="oldExchangeFile"></param>
        /// <param name="newExchangeFile"></param>
        /// <returns></returns>
        public List<FileUserNotification> GetNotoficationUsersByChanges(ExchangeFile oldExchangeFile,
            ExchangeFile newExchangeFile)
        {
            var userNotifications = new List<FileUserNotification>();
            if (oldExchangeFile.AccessDenied)
                return userNotifications;

            IEnumerable<FileNotificationSubscribers> fileNotificationSubscriberses =
                _fileNotificationSubscriberService.GetFileNotificationSubscriberses(oldExchangeFile.FileId);
            if (newExchangeFile == null)
            {
                userNotifications.AddRange(
                    fileNotificationSubscriberses.Select(s => new FileUserNotification()
                    {
                        NotificationType = NotificationType.fileDelited,
                        UserName = s.User.UserName,
                        Email = s.User.UserEmail
                    }));
            }
            else
            {
                if (newExchangeFile.AccessDenied)
                {
                    userNotifications.AddRange(
                        fileNotificationSubscriberses.Select(s => new FileUserNotification()
                        {
                            NotificationType = NotificationType.accessDienied,
                            UserName = s.User.UserName,
                            Email = s.User.UserEmail
                        }));
                }
                else
                {
                    if (oldExchangeFile.Description.ToLower() != newExchangeFile.Description)
                    {
                        userNotifications.AddRange(
                            fileNotificationSubscriberses.Select(s => new FileUserNotification()
                            {
                                NotificationType = NotificationType.descriptionChanged,
                                UserName = s.User.UserName,
                                Email = s.User.UserEmail
                            }));
                    }
                }
            }
            return userNotifications;
        }
    }
}