using System;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Abstract IUnitOfWork base implementation
    /// </summary>
    /// <typeparam name="TContext">Type of database context</typeparam>
    public abstract class UnitOfWorkBase<TContext> : IUnitOfWork
        where TContext : class, IDatabaseContext
    {
        /// <summary>
        /// Database context used by unit of work
        /// </summary>
        protected readonly TContext DatabaseContext;

        /// <summary>
        /// Abstract IUnitOfWork base implementation
        /// </summary>
        /// <param name="databaseContext">Database context to be used by unit of work</param>
        public UnitOfWorkBase(TContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public abstract int SaveChanges();

        public void Dispose()
        {
            DatabaseContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
