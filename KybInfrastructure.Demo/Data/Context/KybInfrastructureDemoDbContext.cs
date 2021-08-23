using KybInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KybInfrastructure.Demo.Data
{
    public partial class KybInfrastructureDemoDbContext : DbContext, IDatabaseContext
    {
        public KybInfrastructureDemoDbContext() { }

        public KybInfrastructureDemoDbContext(DbContextOptions<KybInfrastructureDemoDbContext> options)
            : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=KybInfrastructureDemoDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfiguration<User>(new UserConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
