using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileExchange.Core.DAL.Entity;

namespace FileExchange.Core.Services
{
    public interface IFileNotificationSubscriberService
    {
        FileNotificationSubscribers Add(int userId, int fileId);

        IEnumerable<FileNotificationSubscribers> GetFileNotificationSubscriberses(int fileId); 

        bool UserIsSubscibed(int userId, int fileId);

        void RemoveAll(int fileId);

        void RemoveFromUser(int fileId, int userId);
    }
}
