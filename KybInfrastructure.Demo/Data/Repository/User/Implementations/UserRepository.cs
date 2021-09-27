using KybInfrastructure.Data;

namespace KybInfrastructure.Demo.Data
{
    public class UserRepository : MongoRepository<User>, IUserRepository
    {
        public UserRepository(MongoContext context) : base(context, "User") { }

        public override void Add(User user)
        {
            // Inserts in context avg -> 2.283ms for 10.000 async record
            //                    avg -> 9.841s for 10.000 sync record  
            base.Add(user);
            Context.SaveChanges();
            // Inserts directly avg -> 2.235ms for 10.000 async record
            //                  avg-> 9.538ms for 10.000 async record
            //Collection.InsertOne(user);
        }
    }
}
