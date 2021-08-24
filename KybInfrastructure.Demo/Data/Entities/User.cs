using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KybInfrastructure.Demo.Data
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Token { get; set; }
        public int Role { get; set; }
    }
}
