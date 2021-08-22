using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Entity repository that represents SQL table
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbSet<TEntity> DbSet;

        public EFRepository(DbContext context)
        {
            if (context is null)
                throw new ArgumentNullException("DbContext cannot be null.");
            DbSet = context.Set<TEntity>();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
            => DbSet.Where(filter).FirstOrDefault();

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
            => DbSet.Where(filter).ToList();

        public void Add(TEntity entity)
            => DbSet.Add(entity);

        public void Update(TEntity entity)
            => DbSet.Update(entity);

        public void Remove(TEntity entity)
            => DbSet.Remove(entity);
    }
}
