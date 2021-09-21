using KybInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KybInfrastructure.Demo.Data
{
    public partial class KybInfrastructureDemoDbContext : DbContext, IDatabaseContext
    {
        public KybInfrastructureDemoDbContext() { }

        public KybInfrastructureDemoDbContext(DbContextOptions<KybInfrastructureDemoDbContext> options)
            : base(options) { }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KybInfrastructureDemoDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfiguration<Product>(new ProductConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public bool AreThereAnyChanges()
            => ((DbContext)this).AreThereAnyChanges();

        public void Rollback()
            => ((DbContext)this).Rollback();
    }
}
