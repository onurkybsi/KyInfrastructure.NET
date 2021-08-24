using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace KybInfrastructure.Data
{
    public static class MongoDbExtensions
    {
        public static IMongoCollection<T> CreateCollectionIfNotExists<T>(this IMongoDatabase mongoDatabase, string collectionName, CreateCollectionOptions createCollectionOptions)
        {
            bool collectionIsExists = mongoDatabase.ListCollectionNames(new ListCollectionNamesOptions { Filter = new BsonDocument("name", collectionName) }).Any();

            if (!collectionIsExists)
            {
                if (createCollectionOptions != null)
                {
                    mongoDatabase.CreateCollection(collectionName, createCollectionOptions);
                }
                else
                {
                    mongoDatabase.CreateCollection(collectionName);
                }
            }

            return mongoDatabase.GetCollection<T>(collectionName);
        }

        public static IMongoCollection<T> CreateCollectionIfNotExists<T>(this IMongoDatabase mongoDatabase, string collectionName)
        {
            bool collectionIsExists = mongoDatabase.ListCollectionNames(new ListCollectionNamesOptions { Filter = new BsonDocument("name", collectionName) }).Any();

            if (!collectionIsExists)
                mongoDatabase.CreateCollection(collectionName);

            return mongoDatabase.GetCollection<T>(collectionName);
        }

        public static IMongoCollection<T> CreateUniqueIndex<T>(this IMongoCollection<T> mongoCollection, string fieldName)
        {
            var indexOptions = new CreateIndexOptions { Name = $"UniqueIX_{fieldName}", Unique = true, Sparse = true };
            var model = new CreateIndexModel<T>(new BsonDocument(fieldName, 1), indexOptions);
            mongoCollection.Indexes.CreateOne(model);

            return mongoCollection;
        }
    }
}
