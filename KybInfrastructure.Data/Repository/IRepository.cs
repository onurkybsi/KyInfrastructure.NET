using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Represents data collection in the database (SQL table, NoSQL documents etc.)
    /// </summary>
    /// <typeparam name="TEntity">Database entity type</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets entity by filter
        /// </summary>
        /// <param name="filter">Filter expression of receiving data collection</param>
        /// <returns>Collection of entities</returns>
        TEntity Get(Expression<Func<TEntity, bool>> filter);
        /// <summary>
        /// Gets collection of entities by filter
        /// </summary>
        /// <param name="filter">Filter expression of receiving data collection</param>
        /// <returns>Collection of entities</returns>
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter);
        /// <summary>
        /// Adds new entity to repository
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);
        /// <summary>
        /// Update the existing entity in the repository
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        /// <summary>
        /// Removes entity from repository
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);
    }
}
