using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace KybInfrastructure.Data.Test
{
    public class DbContextExtensionsTest
    {
        private class MockEntity
        {
            [Key]
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class MockDbContext : DbContext
        {
            public MockDbContext(DbContextOptions<MockDbContext> options)
            : base(options) { }

            public virtual DbSet<MockEntity> Entities { get; set; }
        }

        private readonly DbContextOptions<MockDbContext> mockDbContextOption = new DbContextOptionsBuilder<MockDbContext>()
            .UseInMemoryDatabase(databaseName: "MockDatabase")
            .Options;

        private static bool databaseWasSeeded = false;

        public DbContextExtensionsTest()
        {
            if (!databaseWasSeeded)
            {
                using MockDbContext ctx = new(mockDbContextOption);
                SeedDatabase(ctx);
                databaseWasSeeded = true;
            }
        }

        private static void SeedDatabase(MockDbContext ctx)
        {
            ctx.Entities.Add(new MockEntity { Id = 1, Name = "MockEntity1" });
            ctx.Entities.Add(new MockEntity { Id = 2, Name = "MockEntity2" });

            ctx.SaveChanges();
        }

        [Fact]
        public void AreThereAnyChanges_When_A_Entity_Updated_Returns_True()
        {
            using MockDbContext ctx = new(mockDbContextOption);

            MockEntity mockEntity = ctx.Entities.First();
            mockEntity.Name = "Changed";
            ctx.Update(mockEntity);

            Assert.True(ctx.AreThereAnyChanges());
        }

        [Fact]
        public void AreThereAnyChanges_When_A_Entity_Removed_Returns_True()
        {
            using MockDbContext ctx = new(mockDbContextOption);

            MockEntity mockEntity = ctx.Entities.First();
            ctx.Remove(mockEntity);

            Assert.True(ctx.AreThereAnyChanges());
        }

        [Fact]
        public void AreThereAnyChanges_When_A_Entity_Added_Returns_True()
        {
            using MockDbContext ctx = new(mockDbContextOption);

            ctx.Add(new MockEntity { Id = 3, Name = "MockEntity3" });

            Assert.True(ctx.AreThereAnyChanges());
        }

        [Fact]
        public void AreThereAnyChanges_When_There_Is_No_Changes_Along_Context_Lifetime_Returns_False()
        {
            using MockDbContext ctx = new(mockDbContextOption);

            Assert.False(ctx.AreThereAnyChanges());
        }

        [Fact]
        public void Rollback_When_There_Are_Some_Updated_Entry_Sets_EntryState_As_UnModified()
        {
            using MockDbContext ctx = new(mockDbContextOption);
            MockEntity mockEntity = ctx.Entities.First();
            mockEntity.Name = "Changed";
            ctx.Update(mockEntity);

            ctx.Rollback();

            Assert.False(ctx.AreThereAnyChanges());
        }

        [Fact]
        public void Rollback_When_There_Are_Some_Removed_Entry_Sets_EntryState_As_UnModified()
        {
            using MockDbContext ctx = new(mockDbContextOption);
            MockEntity mockEntity = ctx.Entities.First();
            ctx.Remove(mockEntity);

            ctx.Rollback();

            Assert.False(ctx.AreThereAnyChanges());
        }

        [Fact]
        public void Rollback_When_There_Are_Some_Added_Entry_Sets_EntryState_As_UnModified()
        {
            using MockDbContext ctx = new(mockDbContextOption);
            ctx.Add(new MockEntity { Id = 3, Name = "MockEntity3" });

            ctx.Rollback();

            Assert.False(ctx.AreThereAnyChanges());
        }
    }
}
