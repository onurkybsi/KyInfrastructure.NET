using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Base implementation for EF repositories
    /// </summary>
    public class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        protected readonly DbSet<TEntity> DbSet;

        /// <summary>
        /// Base implementation for EF repositories
        /// </summary>
        /// <param name="context">Database context</param>
        protected EFRepository(DbContext context)
        {
            if (context is null)
                throw new ArgumentNullException("DbContext cannot be null.");
            DbSet = context.Set<TEntity>();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
            => FilterDbSetByFilter(filter).FirstOrDefault();

        private IQueryable<TEntity> FilterDbSetByFilter(Expression<Func<TEntity, bool>> filter)
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));
            return DbSet
                    .Where(filter);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
            => FilterDbSetByFilter(filter).ToList();

        public void Add(TEntity entity)
        {
            ValidateEntity(entity);
            DbSet.Add(entity);
        }

        private static void ValidateEntity(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));
        }

        public void Update(TEntity entity)
        {
            ValidateEntity(entity);
            DbSet.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            ValidateEntity(entity);
            DbSet.Remove(entity);
        }
    }
}
