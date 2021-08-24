using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Default IDatabaseContext entity framework implementation
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class EfContext<TDbContext> : DbContext, IDatabaseContext
        where TDbContext : DbContext
    {
        /// <summary>
        /// Default IDatabaseContext entity framework implementation
        /// </summary>
        protected EfContext() { }

        /// <summary>
        /// Default IDatabaseContext entity framework implementation
        /// </summary>
        /// <param name="options">DbContext options</param>
        protected EfContext(DbContextOptions<TDbContext> options) : base(options) { }

        public bool AreThereAnyChanges()
            => this.ChangeTracker
                .Entries()
                .Any(x => x.State == EntityState.Modified ||
                          x.State == EntityState.Added ||
                          x.State == EntityState.Deleted);

        public void Rollback()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}
