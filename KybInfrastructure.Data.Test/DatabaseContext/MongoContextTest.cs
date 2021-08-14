using MongoDB.Driver;
using Moq;
using System;
using Xunit;

namespace KybInfrastructure.Data
{
    public class MongoContextTest
    {
        private readonly Mock<IMongoDatabase> fakeMongoDatabase;
        private readonly MongoContext mongoContext;

        public MongoContextTest()
        {
            fakeMongoDatabase = new Mock<IMongoDatabase>();
            mongoContext = new MongoContext(fakeMongoDatabase.Object);
        }

        [Fact]
        public void GetCollection_Returns_Collection_From_Database_By_Given_Collection_Name()
        {
            // TO-DO
        }

        [Fact]
        public void AddOperation_Throws_Argument_Null_Exception_If_Given_Operation_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => mongoContext.AddOperation(null));
        }

        [Fact]
        public void AddOperation_Adds_Given_Operation_To_Database_Changes_Operations()
        {
            bool called = false;
            Action<IMongoDatabase> toBeAddedOperation = (db) =>
            {
                called = true;
            };

            mongoContext.AddOperation(toBeAddedOperation);
            mongoContext.SaveChanges();

            Assert.True(called);
        }
        
        [Fact]
        public void SaveChanges_Calls_All_Database_Changes_Operations()
        {
            int callCount = 0;
            Action<IMongoDatabase> toBeAddedOperation = (db) =>
            {
                callCount++;
            };
            mongoContext.AddOperation(toBeAddedOperation);
            mongoContext.AddOperation(toBeAddedOperation);
            mongoContext.AddOperation(toBeAddedOperation);
            mongoContext.AddOperation(toBeAddedOperation);

            int databaseChangesOperationsCount = mongoContext.SaveChanges();

            Assert.Equal(4, databaseChangesOperationsCount);
        }
    }
}
