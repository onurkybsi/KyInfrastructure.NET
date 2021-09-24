using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Contains MongoDB.Driver extension methods
    /// </summary>
    public static class MongoDbExtensions
    {
        /// <summary>
        /// Creates a collection by given parameters if not exist
        /// </summary>
        /// <typeparam name="TEntity">Entity type of collection</typeparam>
        /// <param name="mongoDatabase">IMongoDatabase instance</param>
        /// <param name="collectionName">Name of collection</param>
        /// <param name="createCollectionOptions">Creation options of the collection</param>
        /// <returns></returns>
        public static IMongoCollection<TEntity> CreateCollectionIfNotExist<TEntity>(this IMongoDatabase mongoDatabase, string collectionName,
            CreateCollectionOptions createCollectionOptions) where TEntity : class, new()
        {
            ValidateCollectionName(collectionName);
            ValidateCreateCollectionOptions(createCollectionOptions);

            if (!CheckCollectionExistsInDatabase(mongoDatabase, collectionName))
                mongoDatabase.CreateCollection(collectionName, createCollectionOptions);

            return mongoDatabase.GetCollection<TEntity>(collectionName);
        }

        private static bool CheckCollectionExistsInDatabase(IMongoDatabase mongoDatabase, string collectionName)
            => mongoDatabase.ListCollectionNames(new ListCollectionNamesOptions
            {
                Filter = new BsonDocument("name", collectionName)
            })?.Any() ?? false;

        private static void ValidateCollectionName(string collectionName)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentNullException(nameof(collectionName));
        }

        private static void ValidateCreateCollectionOptions(CreateCollectionOptions createCollectionOptions)
        {
            if (createCollectionOptions is null)
                throw new ArgumentNullException(nameof(createCollectionOptions));
        }

        /// <summary>
        /// Creates a collection by given parameters if not exist
        /// </summary>
        /// <typeparam name="TEntity">Entity type of collection</typeparam>
        /// <param name="mongoDatabase">IMongoDatabase instance</param>
        /// <param name="collectionName">Name of collection</param>
        /// <returns></returns>
        public static IMongoCollection<TEntity> CreateCollectionIfNotExist<TEntity>(this IMongoDatabase mongoDatabase, string collectionName)
            where TEntity : class, new()
        {
            ValidateCollectionName(collectionName);

            if (!CheckCollectionExistsInDatabase(mongoDatabase, collectionName))
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
            where TEntity : class, new()
        {
            ValidateFieldName(fieldName);

            CreateIndexOptions indexOptions = BuildCreateIndexOptionsForUniqueIndex(fieldName);

            mongoCollection.Indexes.CreateOne(new CreateIndexModel<TEntity>(new BsonDocument(fieldName, 1), indexOptions));

            return mongoCollection;
        }

        private static void ValidateFieldName(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException(nameof(fieldName));
        }

        private static CreateIndexOptions BuildCreateIndexOptionsForUniqueIndex(string fieldName)
            => new()
            {
                Name = $"UniqueIX_{fieldName}",
                Unique = true,
                Sparse = true
            };
    }
}