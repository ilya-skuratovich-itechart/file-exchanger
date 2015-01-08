using System.Collections.Generic;
using System.Linq;
using Autofac;
using FileExchange.Core.DAL.Entity;
using FileExchange.Core.DAL.Repository;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Services
{
    public class UserRolesService:IUserRolesService
    {
        private IGenericRepository<UserRoles> _repository;

          public UserRolesService(IUnitOfWork unitOfWork)
        {
            _repository = BootStrap.Container.Resolve<IGenericRepository<UserRoles>>();
            _repository.InitializeDbContext(unitOfWork.DbContext);
        }

        public IEnumerable<UserRoles> GetAll()
        {
            return _repository.GetAll();
        }
    }
}