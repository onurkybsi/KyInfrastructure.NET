using KybInfrastructure.Demo.Utils;
using System.Collections.Generic;

namespace KybInfrastructure.Demo.Business
{
    public class UserService : BaseService, IUserService
    {
        public List<User> GetAllUsers()
        {
            IEnumerable<Data.User> allUsersFromDb = UnitOfWork.UserRepository.GetList(user => true);
            return allUsersFromDb.MapTo<List<User>>();
        }

        public List<User> GetAdminUsers()
        {
            IEnumerable<Data.User> allAdminUsersFromDb = UnitOfWork.UserRepository
                .GetList(user => user.Role == (int)UserRole.Admin);
            return allAdminUsersFromDb.MapTo<List<User>>();
        }
    }
}
