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
        protected readonly TContext _context;

        public UnitOfWorkBase(TContext context)
        {
            _context = context;
        }

        public abstract int SaveChanges();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
