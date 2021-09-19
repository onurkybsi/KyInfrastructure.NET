using KybInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace KybInfrastructure.Server.Test
{
    public class UnitOfWorkRegisterTest
    {
        private class MockDatabaseContext : IDatabaseContext
        {
            public bool AreThereAnyChanges()
             => throw new NotImplementedException();

            public void Dispose()
             => throw new NotImplementedException();

            public void Rollback()
            => throw new NotImplementedException();

            public int SaveChanges()
            => throw new NotImplementedException();
        }

        private interface IMockUnitOfWork : IUnitOfWork { }

        private class MockUnitOfWork : IMockUnitOfWork
        {
            public void Dispose()
            => throw new NotImplementedException();

            public int SaveChanges()
            => throw new NotImplementedException();
        }

        private class MockEfContext : DbContext, IDatabaseContext
        {
            public bool AreThereAnyChanges()
                => throw new NotImplementedException();

            public void Rollback()
                => throw new NotImplementedException();
        }

        [Fact]
        public void AddUnitOfWork_Adds_DatabaseContext_Service_To_ServiceCollection()
        {
            ServiceProvider serviceProvider = BuildServiceProvider(serviceCollection =>
                serviceCollection.AddUnitOfWork<MockDatabaseContext, IMockUnitOfWork, MockUnitOfWork>());

            Assert.NotNull(serviceProvider.GetRequiredService<MockDatabaseContext>());
        }

        private static ServiceProvider BuildServiceProvider(Action<IServiceCollection> registrationAction)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            registrationAction(serviceCollection);

            return serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void AddUnitOfWork_Adds_DatabaseContext_Service_To_ServiceCollection_AsScoped()
        {
            ServiceProvider serviceProvider = BuildServiceProvider(serviceCollection =>
                serviceCollection.AddUnitOfWork<MockDatabaseContext, IMockUnitOfWork, MockUnitOfWork>());
            var scope1 = serviceProvider.CreateScope();
            var scope2 = serviceProvider.CreateScope();

            Assert.NotEqual(scope1.ServiceProvider.GetRequiredService<MockDatabaseContext>(),
                scope2.ServiceProvider.GetRequiredService<MockDatabaseContext>());
        }

        [Fact]
        public void AddUnitOfWork_Adds_UnitOfWork_Service_To_ServiceCollection()
        {
            ServiceProvider serviceProvider = BuildServiceProvider(serviceCollection =>
                serviceCollection.AddUnitOfWork<MockDatabaseContext, IMockUnitOfWork, MockUnitOfWork>());

            Assert.NotNull(serviceProvider.GetRequiredService<IMockUnitOfWork>());
        }

        [Fact]
        public void AddUnitOfWork_Adds_UnitOfWork_Service_To_ServiceCollection_AsScoped()
        {
            ServiceProvider serviceProvider = BuildServiceProvider(serviceCollection =>
                serviceCollection.AddUnitOfWork<MockDatabaseContext, IMockUnitOfWork, MockUnitOfWork>());
            var scope1 = serviceProvider.CreateScope();
            var scope2 = serviceProvider.CreateScope();

            Assert.NotEqual(scope1.ServiceProvider.GetRequiredService<MockDatabaseContext>(),
                scope2.ServiceProvider.GetRequiredService<MockDatabaseContext>());
        }

        [Fact]
        public void AddEfUnitOfWork_Adds_DbContext_Service_To_ServiceCollection()
        {
            ServiceProvider serviceProvider = BuildServiceProvider(serviceCollection =>
                serviceCollection.AddEfUnitOfWork<MockEfContext, IMockUnitOfWork, MockUnitOfWork>());

            Assert.NotNull(serviceProvider.GetRequiredService<MockEfContext>());
        }

        [Fact]
        public void AddEfUnitOfWork_Adds_DbContext_Service_To_ServiceCollection_AsScoped()
        {
            ServiceProvider serviceProvider = BuildServiceProvider(serviceCollection =>
                serviceCollection.AddEfUnitOfWork<MockEfContext, IMockUnitOfWork, MockUnitOfWork>());
            var scope1 = serviceProvider.CreateScope();
            var scope2 = serviceProvider.CreateScope();

            Assert.NotEqual(scope1.ServiceProvider.GetRequiredService<MockEfContext>(),
                scope2.ServiceProvider.GetRequiredService<MockEfContext>());
        }

        [Fact]
        public void AddEfUnitOfWork_Adds_UnitOfWork_Service_To_ServiceCollection()
        {
            ServiceProvider serviceProvider = BuildServiceProvider(serviceCollection =>
                serviceCollection.AddEfUnitOfWork<MockEfContext, IMockUnitOfWork, MockUnitOfWork>());

            Assert.NotNull(serviceProvider.GetRequiredService<IMockUnitOfWork>());
        }

        [Fact]
        public void AddEfUnitOfWork_Adds_UnitOfWork_Service_To_ServiceCollection_AsScoped()
        {
            ServiceProvider serviceProvider = BuildServiceProvider(serviceCollection =>
                serviceCollection.AddEfUnitOfWork<MockEfContext, IMockUnitOfWork, MockUnitOfWork>());
            var scope1 = serviceProvider.CreateScope();
            var scope2 = serviceProvider.CreateScope();

            Assert.NotEqual(scope1.ServiceProvider.GetRequiredService<MockEfContext>(),
                scope2.ServiceProvider.GetRequiredService<MockEfContext>());
        }
    }
}
