using System.Collections.Generic;
using FileExchange.Core.DAL.Entity;

namespace FileExchange.Core.Services
{
    public interface IUserProfileService
    {
        IEnumerable<UserProfile> GetUsers();

        IEnumerable<UserProfile> GetUsersPaged(int startPage, int length, out int totalRecords);

        void Update(UserProfile userProfile);

        UserProfile GetUserById(int userId);

        UserProfile GetUserByName(string userName);
    }
}