using System;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Interface of a database context which has required assests of unit of work
    /// </summary>
    public interface IDatabaseContext : IDisposable
    {
        /// <summary>
        /// Returns whether changes exists or not in database
        /// </summary>
        /// <returns>Changes exists or not</returns>
        bool AreThereAnyChanges();

        /// <summary>
        /// Rollbacks changes without saving
        /// </summary>
        void Rollback();

        /// <summary>
        /// It ensures that the changed database assests are written to the database permanently, ends the transaction.
        /// </summary>
        /// <returns>Number of changed database entry</returns>
        int SaveChanges();
    }
}
