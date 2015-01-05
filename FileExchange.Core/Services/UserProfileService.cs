using System.Collections.Generic;
using System.Linq;
using Autofac;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Repositories;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Services
{
    public class UserProfileService : IUserProfileService
    {

        private IGenericRepository<UserProfile> _userProfileRepository;

        public UserProfileService(IUnitOfWork unitOfWork)
        {
            _userProfileRepository = BootStrap.Container.Resolve<IGenericRepository<UserProfile>>();
            _userProfileRepository.InitializeDbContext(unitOfWork.DbContext);

        }

        public IEnumerable<UserProfile> GetUsers()
        {
            return _userProfileRepository.GetAll();
        }

        public IEnumerable<UserProfile> GetUsersPaged(int startPage, int length, out int totalRecords)
        {

            return _userProfileRepository
                .GetPaged(u => u.UserId, startPage == 0 ? 1 : startPage, length, out totalRecords)
                .ToList();
        }

        public UserProfile GetUserById(int userId)
        {
            return _userProfileRepository.FindBy(u => u.UserId == userId)
                .SingleOrDefault();
        }

        public UserProfile GetUserByName(string userName)
        {
            return _userProfileRepository.FindBy(u => u.UserName == userName)
                .SingleOrDefault();
        }

        public void Update(UserProfile userProfile)
        {
            _userProfileRepository.Update(userProfile);
        }
    }
}