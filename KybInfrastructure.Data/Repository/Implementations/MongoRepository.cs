using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Base implementation for MongoDb repositories
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class MongoRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly string _collectionName;

        /// <summary>
        /// MongoContext instance
        /// </summary>
        protected readonly MongoContext Context;

        /// <summary>
        /// Represents the collection of entity which is in the database
        /// </summary>
        protected readonly IMongoCollection<TEntity> Collection;

        /// <summary>
        /// Base implementation for MongoDb repositories
        /// </summary>
        /// <param name="context">MongoContext of the transaction</param>
        /// <param name="collectionName">Name of the collection in the database</param>
        protected MongoRepository(MongoContext context, string collectionName)
        {
            Context = context;
            _collectionName = collectionName;
            Collection = Context.GetCollection<TEntity>(collectionName);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
            => filter is null
                ? default
                : Collection.Find(new ExpressionFilterDefinition<TEntity>(filter)).FirstOrDefault();

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
            => filter is null
                ? default
                : Collection.Find(new ExpressionFilterDefinition<TEntity>(filter)).ToList();

        public virtual void Add(TEntity entity)
            => Context.AddOperation((db) =>
            {
                db.GetCollection<TEntity>(_collectionName).InsertOne(entity);
            });

        public virtual void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
