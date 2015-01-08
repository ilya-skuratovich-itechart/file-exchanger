using System.Collections.Generic;
using FileExchange.Core.DAL.Entity;

namespace FileExchange.Core.Services
{
    public interface IUserRolesService
    {
        IEnumerable<UserRoles> GetAll();
    }
}