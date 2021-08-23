using System.Collections.Generic;

namespace KybInfrastructure.Demo.Business
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        List<User> GetAdminUsers();
    }
}
