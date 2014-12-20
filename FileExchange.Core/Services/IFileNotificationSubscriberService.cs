using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core.Services
{
    public interface IFileNotificationSubscriberService
    {
        FileNotificationSubscribers Add(int userId, int fileId);

        bool UserIsSubscibed(int userId, int fileId);

        void RemoveAll(int fileId);

        void RemoveFromUser(int fileId, int userId);
    }
}
