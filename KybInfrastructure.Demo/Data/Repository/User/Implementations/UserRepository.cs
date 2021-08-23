using KybInfrastructure.Data;

namespace KybInfrastructure.Demo.Data
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(KybInfrastructureDemoDbContext dbContext) : base(dbContext) { }
    }
}
