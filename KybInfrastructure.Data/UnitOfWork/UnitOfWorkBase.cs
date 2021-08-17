using System;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Generic abstract IUnitOfWork base implementation
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class UnitOfWorkBase<TContext> : IUnitOfWork
        where TContext : class, IDatabaseContext, new()
    {
        protected readonly TContext DatabaseContext;

        public UnitOfWorkBase(TContext context)
        {
            DatabaseContext = context;
        }

        public abstract int SaveChanges();

        public void Dispose()
        {
            DatabaseContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
