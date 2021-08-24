using MongoDB.Driver;
using System;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Context interface that needs to be created to adapt the changes of data in MongoDB to the unit of work.
    /// </summary>
    public interface IMongoContext : IDatabaseContext
    {
        /// <summary>
        /// Adds a data change operation to the created database context
        /// </summary>
        /// <param name="operation">Database changing operation</param>
        void AddOperation(Action<IMongoDatabase> operation);
    }
}
