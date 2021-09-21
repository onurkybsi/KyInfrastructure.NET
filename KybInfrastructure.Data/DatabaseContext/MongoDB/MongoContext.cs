using KybInfrastructure.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Default IMongoContext implementation
    /// </summary>
    public class MongoContext : IMongoContext
    {
        private bool _thereAreChanges;

        private readonly IMongoDatabase _databaseAccessor;

        private readonly List<Action<IMongoDatabase>> _changeOperations = new();

        /// <summary>
        /// Default IMongoContext implementation
        /// </summary>
        /// <param name="databaseAccessor">IMongoDatabase object that provide access to database</param>
        public MongoContext(IMongoDatabase databaseAccessor)
        {
            if (databaseAccessor is null)
                throw new ArgumentNullException(nameof(databaseAccessor));

            _databaseAccessor = databaseAccessor;
        }

        /// <summary>
        /// Returns the collection in the database according to the given collection name
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="collectionName">The name of the collection to be accessed</param>
        /// <returns></returns>
        public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new InvalidArgumentException(nameof(collectionName), collectionName);
            return _databaseAccessor.GetCollection<TEntity>(collectionName);
        }

        public void AddOperation(Action<IMongoDatabase> operation)
        {
            if (operation is null)
                throw new ArgumentNullException(nameof(operation));

            _changeOperations.Add(operation);
            _thereAreChanges = true;
        }

        public bool AreThereAnyChanges()
            => _thereAreChanges;

        public void Rollback()
            => ResetContext();

        private void ResetContext()
        {
            _changeOperations.Clear();
            _thereAreChanges = false;
        }

        public int SaveChanges()
        {
            int changedEntry = _changeOperations.Count;
            foreach (var operation in _changeOperations)
            {
                operation.Invoke(_databaseAccessor);
            }
            ResetContext();

            return changedEntry;
        }

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}
