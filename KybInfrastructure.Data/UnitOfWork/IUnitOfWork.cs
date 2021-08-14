using System;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Design pattern that manages data transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// It ensures that the changed database items are written to the database permanently, ends the transaction.
        /// </summary>
        /// <returns>Number of changed database entry</returns>
        int SaveChanges();
    }
}
