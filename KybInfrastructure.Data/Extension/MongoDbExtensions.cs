using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Contains MongoDB.Driver extension methods
    /// </summary>
    public static class MongoDbExtensions
    {
        /// <summary>
        /// Creates a collection by given parameters if not exists
        /// </summary>
        /// <typeparam name="TEntity">Entity type of collection</typeparam>
        /// <param name="mongoDatabase">IMongoDatabase instance</param>
        /// <param name="collectionName">Name of collection</param>
        /// <param name="createCollectionOptions">Creation options of the collection</param>
        /// <returns></returns>
        public static IMongoCollection<TEntity> CreateCollectionIfNotExists<TEntity>(this IMongoDatabase mongoDatabase, string collectionName, CreateCollectionOptions createCollectionOptions)
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

            return mongoDatabase.GetCollection<TEntity>(collectionName);
        }

        /// <summary>
        /// Creates a collection by given parameters if not exists
        /// </summary>
        /// <typeparam name="TEntity">Entity type of collection</typeparam>
        /// <param name="mongoDatabase">IMongoDatabase instance</param>
        /// <param name="collectionName">Name of collection</param>
        /// <returns></returns>
        public static IMongoCollection<TEntity> CreateCollectionIfNotExists<TEntity>(this IMongoDatabase mongoDatabase, string collectionName)
        {
            bool collectionIsExists = mongoDatabase.ListCollectionNames(new ListCollectionNamesOptions { Filter = new BsonDocument("name", collectionName) }).Any();

            if (!collectionIsExists)
                mongoDatabase.CreateCollection(collectionName);

            return mongoDatabase.GetCollection<TEntity>(collectionName);
        }

        /// <summary>
        /// Creates a unique index for the field
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="mongoCollection">IMongoCollection instance</param>
        /// <param name="fieldName">Name of field</param>
        /// <returns></returns>
        public static IMongoCollection<TEntity> CreateUniqueIndex<TEntity>(this IMongoCollection<TEntity> mongoCollection, string fieldName)
        {
            var indexOptions = new CreateIndexOptions { Name = $"UniqueIX_{fieldName}", Unique = true, Sparse = true };
            var model = new CreateIndexModel<TEntity>(new BsonDocument(fieldName, 1), indexOptions);
            mongoCollection.Indexes.CreateOne(model);

            return mongoCollection;
        }
    }
}
