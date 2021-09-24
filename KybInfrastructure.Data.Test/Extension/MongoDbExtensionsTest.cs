using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Bindings;
using MongoDB.Driver.Core.Operations;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders;
using Moq;
using System;
using Xunit;

namespace KybInfrastructure.Data.Test
{
    public class MongoDbExtensionsTest
    {
        public class MockEntity { }

        private readonly Mock<IMongoDatabase> mockMongoDatabase;

        public MongoDbExtensionsTest()
        {
            mockMongoDatabase = new Mock<IMongoDatabase>();
        }

        [Fact]
        public void CreateCollectionIfNotExists_Throws_ArgumentNullException_If_Given_Collection_Name_Is_Null()
        {
            string collectionName = null;

            Assert.Throws<ArgumentNullException>(() =>
                mockMongoDatabase.Object.CreateCollectionIfNotExist<MockEntity>(collectionName, new CreateCollectionOptions()));
        }

        [Fact]
        public void CreateCollectionIfNotExists_Throws_ArgumentNullException_If_Given_Collection_Name_Is_Empty()
        {
            string collectionName = " ";

            Assert.Throws<ArgumentNullException>(() =>
                mockMongoDatabase.Object.CreateCollectionIfNotExist<MockEntity>(collectionName, new CreateCollectionOptions()));
        }

        [Fact]
        public void CreateCollectionIfNotExists_Throws_ArgumentNullException_If_Given_CreateCollectionOptions_Is_Null()
        {
            CreateCollectionOptions createCollectionOptions = null;

            Assert.Throws<ArgumentNullException>(() =>
                mockMongoDatabase.Object.CreateCollectionIfNotExist<MockEntity>("collectionName", createCollectionOptions));
        }

        [Fact]
        public void CreateCollectionIfNotExists_Creates_A_Collection_With_The_Name_Given_If_Not_Exist()
        {
            mockMongoDatabase.Setup(md => md.ListCollectionNames(It.IsAny<ListCollectionNamesOptions>(), default))
                .Returns(default(IAsyncCursor<string>));
            mockMongoDatabase.Setup(md => md.GetCollection<MockEntity>("collectionName", default))
                .Returns(It.IsAny<IMongoCollection<MockEntity>>());
            CreateCollectionOptions options = new();

            mockMongoDatabase.Object.CreateCollectionIfNotExist<MockEntity>("collectionName", options);

            mockMongoDatabase.Verify(md => md.CreateCollection("collectionName", options, default), Times.Once);
        }

        [Fact]
        public void CreateCollectionIfNotExists_Doesnt_Create_Any_Collection_If_There_Is_A_Collection_With_The_Name_Given()
        {
            mockMongoDatabase.Setup(md => md.ListCollectionNames(It.IsAny<ListCollectionNamesOptions>(), default))
                .Returns(CreateCursor());
            mockMongoDatabase.Setup(md => md.GetCollection<MockEntity>("collectionName", default))
                .Returns(It.IsAny<IMongoCollection<MockEntity>>());
            CreateCollectionOptions options = new();

            mockMongoDatabase.Object.CreateCollectionIfNotExist<MockEntity>("collectionName", options);

            mockMongoDatabase.Verify(md => md.CreateCollection("collectionName", options, default), Times.Never);
        }

        private static IAsyncCursor<string> CreateCursor()
        {
            return new AsyncCursor<string>(
                channelSource: new Mock<IChannelSource>().Object,
                collectionNamespace: new CollectionNamespace("foo", "bar"),
                query: new BsonDocument(),
                firstBatch: new string[] { "collectionName" },
                cursorId: 0,
                batchSize: null,
                limit: null,
                serializer: new Mock<IBsonSerializer<string>>().Object,
                messageEncoderSettings: new MessageEncoderSettings(),
                maxTime: null);
        }

        [Fact]
        public void CreateCollectionIfNotExists_Returns_The_Collection_With_The_Name_Given()
        {
            mockMongoDatabase.Setup(md => md.ListCollectionNames(It.IsAny<ListCollectionNamesOptions>(), default))
                .Returns(CreateCursor());
            mockMongoDatabase.Setup(md => md.GetCollection<MockEntity>("collectionName", default))
                .Returns(It.IsAny<IMongoCollection<MockEntity>>());
            mockMongoDatabase.Setup(md => md.GetCollection<MockEntity>("collectionName", default))
                .Returns(new Mock<IMongoCollection<MockEntity>>().Object);
            CreateCollectionOptions options = new();

            IMongoCollection<MockEntity> collection = mockMongoDatabase.Object.CreateCollectionIfNotExist<MockEntity>("collectionName", options);

            Assert.NotNull(collection);
        }

        [Fact]
        public void CreateUniqueIndex_Throws_ArgumentNullException_If_Given_FieldName_Is_Null()
        {
            Mock<IMongoCollection<MockEntity>> mockMongoCollection = new();

            Assert.Throws<ArgumentNullException>(() => mockMongoCollection.Object.CreateUniqueIndex<MockEntity>(null));
        }

        [Fact]
        public void CreateUniqueIndex_Throws_ArgumentNullException_If_Given_FieldName_Is_Empty()
        {
            Mock<IMongoCollection<MockEntity>> mockMongoCollection = new();

            Assert.Throws<ArgumentNullException>(() => mockMongoCollection.Object.CreateUniqueIndex<MockEntity>(" "));
        }

        [Fact]
        public void CreateUniqueIndex_Calls_IMongoIndexManager_CreateOne_To_CreateIndex()
        {
            (Mock<IMongoCollection<MockEntity>> mockMongoCollection, Mock<IMongoIndexManager<MockEntity>> mockMongoIndexManager)
                = CreateMockMongoCollection();

            IMongoCollection<MockEntity> collection = mockMongoCollection.Object.CreateUniqueIndex<MockEntity>("indexName");

            mockMongoIndexManager.Verify(mim => mim.CreateOne(It.IsAny<CreateIndexModel<MockEntity>>(), default, default), Times.Once);
        }

        private static Tuple<Mock<IMongoCollection<MockEntity>>, Mock<IMongoIndexManager<MockEntity>>> CreateMockMongoCollection()
        {
            Mock<IMongoCollection<MockEntity>> mockMongoCollection = new();
            Mock<IMongoIndexManager<MockEntity>> mockMongoIndexManager = new();
            mockMongoIndexManager.Setup(mim => mim.CreateOne(It.IsAny<CreateIndexModel<MockEntity>>(), default, default));
            mockMongoCollection.Setup(mc => mc.Indexes)
                .Returns(mockMongoIndexManager.Object);

            return new Tuple<Mock<IMongoCollection<MockEntity>>, Mock<IMongoIndexManager<MockEntity>>>(mockMongoCollection, mockMongoIndexManager);
        }

        [Fact]
        public void CreateUniqueIndex_Returns_Collection_Which_Has_New_UniqueIndex()
        {
            (Mock<IMongoCollection<MockEntity>> mockMongoCollection, Mock<IMongoIndexManager<MockEntity>> mockMongoIndexManager)
                = CreateMockMongoCollection();

            IMongoCollection<MockEntity> collection = mockMongoCollection.Object.CreateUniqueIndex<MockEntity>("indexName");

            Assert.Equal(mockMongoCollection.Object, collection);
        }
    }
}