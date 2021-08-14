using Moq;
using Xunit;

namespace KybInfrastructure.Data.Test
{
    public class UnitOfWorkBaseTest
    {
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
