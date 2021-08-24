using KybInfrastructure.Data;

namespace KybInfrastructure.Demo.Data
{
    public class UserRepository : MongoRepository<User>, IUserRepository
    {
        private const string USER_COLLECTION_NAME = "User";

        public UserRepository(MongoContext context) : base(context, USER_COLLECTION_NAME) { }

        public override void Add(User user)
            => Collection.InsertOne(user);
    }
}
