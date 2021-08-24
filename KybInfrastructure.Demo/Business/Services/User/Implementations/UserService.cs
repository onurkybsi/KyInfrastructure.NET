using KybInfrastructure.Demo.Data;
using KybInfrastructure.Demo.Utils;
using System.Collections.Generic;

namespace KybInfrastructure.Demo.Business
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllUsers()
        {
            IEnumerable<Data.User> allUsersFromDb = _userRepository
                .GetList(user => true);
            return allUsersFromDb.MapTo<List<User>>();
        }

        public List<User> GetAdminUsers()
        {
            IEnumerable<Data.User> allAdminUsersFromDb = _userRepository
                .GetList(user => user.Role == (int)UserRole.Admin);
            return allAdminUsersFromDb.MapTo<List<User>>();
        }

        public void AddUser(User user)
            => _userRepository.Add(user.MapTo<Data.User>());
    }
}
