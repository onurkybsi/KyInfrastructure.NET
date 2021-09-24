using Moq;
using Xunit;

namespace KybInfrastructure.Data.Test
{
    public class UnitOfWorkBaseTest
    {
        public class FakeDatabaseContext : IDatabaseContext
        {
            public bool AreThereAnyChanges()
                => throw new System.NotImplementedException();

            public virtual void Dispose() { }

            public void Rollback()
                => throw new System.NotImplementedException();

            public int SaveChanges()
                => throw new System.NotImplementedException();
        }

        private readonly Mock<FakeDatabaseContext> fakeDatabaseContext;
        private readonly Mock<UnitOfWorkBase<FakeDatabaseContext>> fakeUnitOfWork;

        public UnitOfWorkBaseTest()
        {
            fakeDatabaseContext = new Mock<FakeDatabaseContext>();
            fakeUnitOfWork = new Mock<UnitOfWorkBase<FakeDatabaseContext>>(fakeDatabaseContext.Object);
        }

        [Fact]
        public void Dispose_Disposes_DatabaseContext()
        {
            fakeDatabaseContext.Setup(ctx => ctx.Dispose());

            fakeUnitOfWork.Object.Dispose();

            fakeDatabaseContext.Verify(ctx => ctx.Dispose());
        }
    }
}
