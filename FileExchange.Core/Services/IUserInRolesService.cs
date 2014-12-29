using System.Collections.Generic;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core.Services
{
    public interface IUserInRolesService
    {
        IEnumerable<UserInRoles> GetUserInRoles(int userId);

        /// <summary>
        /// adding not exists roles to user and remove not selected roles/
        /// </summary>
        /// <param name="userInRolesIds"></param>
        void UpdateUserInRoles(int userId, List<int> userInRolesIds);

        UserInRoles AddUserToRole(int userId, UserRoleTypes userRoleType);
    }
}