using KybInfrastructure.Core;
using MongoDB.Driver;
using Moq;
using System;
using Xunit;

namespace KybInfrastructure.Data
{
    public class MongoContextTest
    {
        public class MockEntity { }

        private readonly Mock<IMongoDatabase> fakeMongoDatabase;

        public MongoContextTest()
        {
            fakeMongoDatabase = new Mock<IMongoDatabase>();
        }

        [Fact]
        public void MongoContext_Throws_ArgumentNullException_If_Given_IMongoDatabase_Is_Null()
            => Assert.Throws<ArgumentNullException>(() => new MongoContext(null));

        [Fact]
        public void GetCollection_Throws_InvalidArgumentException_If_Given_Collection_Name_Is_Null()
        {
            MongoContext ctx = new(fakeMongoDatabase.Object);

            Assert.Throws<InvalidArgumentException>(() => { ctx.GetCollection<MockEntity>(null); });
        }

        [Fact]
        public void GetCollection_Throws_InvalidArgumentException_If_Given_Collection_Name_Is_Empty()
        {
            MongoContext ctx = new(fakeMongoDatabase.Object);

            Assert.Throws<InvalidArgumentException>(() => { ctx.GetCollection<MockEntity>(" "); });
        }

        [Fact]
        public void GetCollection_Returns_Collection_From_Database_By_Given_Collection_Name()
        {
            Mock<IMongoCollection<MockEntity>> mockCollection = new();
            fakeMongoDatabase.Setup(db => db.GetCollection<MockEntity>("MockDatabase", null))
                .Returns(mockCollection.Object);
            MongoContext ctx = new(fakeMongoDatabase.Object);

            var collection = ctx.GetCollection<MockEntity>("MockDatabase");

            Assert.Equal(mockCollection.Object, collection);
        }

        [Fact]
        public void AddOperation_Throws_ArgumentNullException_If_Given_Operation_Is_Null()
        {
            MongoContext mongoContext = new(fakeMongoDatabase.Object);

            Assert.Throws<ArgumentNullException>(() => mongoContext.AddOperation(null));
        }

        [Fact]
        public void AddOperation_Adds_Given_Operation_To_Database_Changes_Operations()
        {
            MongoContext mongoContext = new(fakeMongoDatabase.Object);
            bool flag = false;
            void someOperation(IMongoDatabase db) => flag = true;

            mongoContext.AddOperation(someOperation);
            mongoContext.SaveChanges();

            Assert.True(flag);
        }

        [Fact]
        public void AddOperation_Set_AreThereAnyChanges_Flag_True()
        {
            MongoContext mongoContext = new(fakeMongoDatabase.Object);

            mongoContext.AddOperation((mongoDb) => { });

            Assert.True(mongoContext.AreThereAnyChanges());
        }

        [Fact]
        public void SaveChanges_Invokes_All_Database_Change_Operations()
        {
            MongoContext mongoContext = new(fakeMongoDatabase.Object);
            int invokingCount = 0;
            void aOperation(IMongoDatabase db) => invokingCount++;
            mongoContext.AddOperation(aOperation);
            mongoContext.AddOperation(aOperation);
            mongoContext.AddOperation(aOperation);
            mongoContext.AddOperation(aOperation);

            int databaseChangesOperationsCount = mongoContext.SaveChanges();

            Assert.Equal(4, databaseChangesOperationsCount);
        }

        [Fact]
        public void SaveChanges_Set_AreThereAnyChanges_Flag_False()
        {
            MongoContext mongoContext = new(fakeMongoDatabase.Object);
            static void aOperation(IMongoDatabase db) { };
            mongoContext.AddOperation(aOperation);

            mongoContext.SaveChanges();

            Assert.False(mongoContext.AreThereAnyChanges());
        }

        [Fact]
        public void AreThereAnyChanges_Returns_False_If_There_Is_No_Change_Operation_Added()
        {
            MongoContext mongoContext = new(fakeMongoDatabase.Object);

            Assert.False(mongoContext.AreThereAnyChanges());
        }

        [Fact]
        public void AreThereAnyChanges_Returns_True_If_There_Is_A_Change_Operation_Added()
        {
            MongoContext mongoContext = new(fakeMongoDatabase.Object);
            static void aOperation(IMongoDatabase db) { };
            mongoContext.AddOperation(aOperation);

            Assert.True(mongoContext.AreThereAnyChanges());
        }
    }
}
