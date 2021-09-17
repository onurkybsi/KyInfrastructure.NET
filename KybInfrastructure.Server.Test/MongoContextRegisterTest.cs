using KybInfrastructure.Core;
using KybInfrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace KybInfrastructure.Server.Test
{
    public class MongoContextRegisterTest
    {
        [Fact]
        public void AddMongoContext_Throws_InvalidArgumentException_If_Given_MongoDbConnectionString_Is_Null()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            Assert.Throws<InvalidArgumentException>(() => serviceCollection.AddMongoContext(null, "SomeValue"));
        }

        [Fact]
        public void AddMongoContext_Throws_InvalidArgumentException_If_Given_MongoDbConnectionString_Is_Empty()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            Assert.Throws<InvalidArgumentException>(() => serviceCollection.AddMongoContext(" ", "SomeValue"));
        }

        [Fact]
        public void AddMongoContext_Throws_InvalidArgumentException_If_Given_DatabaseName_Is_Null()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            Assert.Throws<InvalidArgumentException>(() => serviceCollection.AddMongoContext("SomeValue", null));
        }

        [Fact]
        public void AddMongoContext_Throws_InvalidArgumentException_If_Given_DatabaseName_Is_Empty()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            Assert.Throws<InvalidArgumentException>(() => serviceCollection.AddMongoContext("SomeValue", " "));
        }

        [Fact]
        public void AddMongoContext_Registers_IMongoClient_Service()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddMongoContext("connectionString", "databaseName");

            Assert.Contains(serviceCollection, service => service.ServiceType == typeof(IMongoClient));
        }

        [Fact]
        public void AddMongoContext_MongoContext_Service()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddMongoContext("connectionString", "databaseName");

            Assert.Contains(serviceCollection, service => service.ServiceType == typeof(MongoContext));
        }
    }
}