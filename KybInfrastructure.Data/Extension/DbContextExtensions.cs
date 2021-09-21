using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KybInfrastructure.Data
{
    /// <summary>
    /// Contains extension methods of DbContext
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Returns whether changes exists or not in database
        /// </summary>
        /// <returns>Changes exists or not</returns>
        public static bool AreThereAnyChanges(this DbContext context)
            => context.ChangeTracker
                .Entries()
                .Any(x => x.State == EntityState.Modified ||
                          x.State == EntityState.Added ||
                          x.State == EntityState.Deleted);

        /// <summary>
        /// Rollbacks changes without saving
        /// </summary>
        public static void Rollback(this DbContext context)
        {
            var changedEntries = context.ChangeTracker
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
