using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KybInfrastructure.Data
{
    public class MongoRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly MongoContext _context;
        private readonly string _collectionName;
        protected readonly IMongoCollection<TEntity> Collection;

        protected MongoRepository(MongoContext context, string collectionName)
        {
            _context = context;
            _collectionName = collectionName;
            Collection = _context.GetCollection<TEntity>(collectionName);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
            => filter is null
                ? default
                : Collection.Find(new ExpressionFilterDefinition<TEntity>(filter)).FirstOrDefault();

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
            => filter is null
                ? default
                : Collection.Find(new ExpressionFilterDefinition<TEntity>(filter)).ToList();

        public void Add(TEntity entity)
            => _context.AddOperation((db) =>
            {
                db.GetCollection<TEntity>(_collectionName).InsertOne(entity);
            });

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
