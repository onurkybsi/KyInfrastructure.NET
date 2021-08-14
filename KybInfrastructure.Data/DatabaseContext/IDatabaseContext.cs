using System;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Interface of database context required for unit of work design pattern
    /// </summary>
    public interface IDatabaseContext : IDisposable
    {
        /// <summary>
        /// It ensures that the changed database items are written to the database permanently, ends the transaction.
        /// </summary>
        /// <returns>Number of changed database entry</returns>
        int SaveChanges();
    }
}
