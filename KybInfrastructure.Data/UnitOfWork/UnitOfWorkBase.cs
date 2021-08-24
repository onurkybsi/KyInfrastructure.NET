using System;
using System.Linq;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Abstract IUnitOfWork base implementation
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        /// <summary>
        /// Database contexts used by unit of work
        /// </summary>
        protected readonly IDatabaseContext[] DatabaseContexts;

        /// <summary>
        /// Abstract IUnitOfWork base implementation
        /// </summary>
        /// <param name="databaseContexts">Database contexts to be used by unit of work</param>
        public UnitOfWorkBase(params IDatabaseContext[] databaseContexts)
        {
            DatabaseContexts = databaseContexts;
        }

        /// <summary>
        /// Returns existing database context by type
        /// </summary>
        /// <typeparam name="TContext">Database context type</typeparam>
        /// <returns>Database context</returns>
        protected TContext GetContext<TContext>()
            => (TContext)DatabaseContexts.FirstOrDefault(context => context.GetType() == typeof(TContext));

        /// <summary>
        /// Returns existing database contexts that have changes
        /// </summary>
        /// <returns>Database contexts</returns>
        protected IDatabaseContext[] GetContextsThatHaveChanges()
            => DatabaseContexts
                .Where(context => context.AreThereAnyChanges())
                .ToArray();

        public abstract int SaveChanges();

        public void Dispose()
        {
            foreach (var databaseContext in DatabaseContexts)
                databaseContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
