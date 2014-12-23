using System.Collections.Generic;
using System.Linq;
using Autofac;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Repositories;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Services
{
    public class UserInRolesService : IUserInRolesService
    {
        private IGenericRepository<UserInRoles> _userInRolesRepository;

        public UserInRolesService(IUnitOfWork unitOfWork)
        {
            _userInRolesRepository = BootStrap.Container.Resolve<IGenericRepository<UserInRoles>>();
            _userInRolesRepository.InitializeDbContext(unitOfWork.DbContext);
        }

        public IEnumerable<UserInRoles> GetUserInRoles(int userId)
        {
            return _userInRolesRepository.FindBy(u => u.UserId == userId);
        }

        public void UpdateUserInRoles(int userId,List<int> userInRolesIds)
        {
            if (userInRolesIds != null)
            {
               var existsUserRoles= _userInRolesRepository.FindBy(u => u.UserId == userId).ToList();

                IEnumerable<int> userRolesToAdd =
                    userInRolesIds.Where(r => !existsUserRoles.Any(e => e.RoleId == r));

                IEnumerable<UserInRoles> userInRolesToRemove = existsUserRoles.Where(r => !userInRolesIds.Any(e => e == r.RoleId));
                foreach (var userInRole in userInRolesToRemove)
                    _userInRolesRepository.Delete(userInRole);

                foreach (var userRoleId in userRolesToAdd)
                {
                    _userInRolesRepository.Add(new UserInRoles() {UserId = userId, RoleId = userRoleId});
                }
            }
        }
    }
}