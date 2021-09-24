using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace KybInfrastructure.Data.Test
{
    public class EFRepositoryTest
    {
        public class MockEntity
        {
            [Key]
            public int Id { get; set; }
        };

        public class MockRepository : EFRepository<MockEntity>
        {
            public MockRepository(DbContext context) : base(context) { }
        }

        private readonly Mock<DbContext> mockDbContext;

        public EFRepositoryTest()
        {
            mockDbContext = new Mock<DbContext>();
        }

        [Fact]
        public void EFRepository_Throws_ArgumentNullException_If_Given_DbContext_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new DbContext(default));
        }

        [Fact]
        public void EFRepository_Set_DbSet()
        {
            mockDbContext.Setup(ctx => ctx.Set<MockEntity>());

            MockRepository eFRepository = new(mockDbContext.Object);

            mockDbContext.Verify(ctx => ctx.Set<MockEntity>(), Times.Once);
        }

        [Fact]
        public void Get_Throws_ArgumentNullException_If_Given_Filter_Value_Is_Null()
        {
            mockDbContext.Setup(ctx => ctx.Set<MockEntity>());
            MockRepository eFRepository = new(mockDbContext.Object);

            Assert.Throws<ArgumentNullException>(() => eFRepository.Get(null));
        }

        [Fact]
        public void Get_Returns_First_Value_Fit_The_Filter_Given_If_Exist_In_DbSet()
        {
            List<MockEntity> mockDbSetSource = new()
            {
                new MockEntity { Id = 1 }
            };
            MockRepository eFRepository = CreateMockRepositoryWithSourceList(mockDbSetSource);

            MockEntity entity = eFRepository.Get(entity => entity.Id == 1);

            Assert.Equal(mockDbSetSource[0], entity);
        }

        private MockRepository CreateMockRepositoryWithSourceList(List<MockEntity> sourceList)
        {
            DbSet<MockEntity> mockDbSet = CreateMockDbSet<MockEntity>(sourceList);
            mockDbContext.Setup(ctx => ctx.Set<MockEntity>())
                .Returns(mockDbSet);
            MockRepository eFRepository = new(mockDbContext.Object);

            return eFRepository;
        }

        private static DbSet<T> CreateMockDbSet<T>(List<T> sourceList)
            where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            dbSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((s) => sourceList.Remove(s));
            dbSet.Setup(d => d.Update(It.IsAny<T>()));

            return dbSet.Object;
        }

        [Fact]
        public void Get_Returns_Null_If_There_Is_No_Value_Fit_The_Filter_Given_In_DbSet()
        {
            List<MockEntity> mockDbSetSource = new()
            {
                new MockEntity { Id = 1 }
            };
            MockRepository eFRepository = CreateMockRepositoryWithSourceList(mockDbSetSource);

            MockEntity entity = eFRepository.Get(entity => entity.Id == 2);

            Assert.Equal(default(MockEntity), entity);
        }

        [Fact]
        public void GetList_Returns_List_Of_Values_Fit_The_Filter_Given_If_Exist_In_DbSet()
        {
            List<MockEntity> mockDbSetSource = new()
            {
                new MockEntity { Id = 1 },
                new MockEntity { Id = 2 }
            };
            MockRepository eFRepository = CreateMockRepositoryWithSourceList(mockDbSetSource);

            List<MockEntity> entities = eFRepository.GetList(entity => entity.Id >= 1).ToList();

            Assert.True(entities[0] == mockDbSetSource[0] && entities[1] == mockDbSetSource[1]);
        }

        [Fact]
        public void GetList_Returns_EmptyList_If_There_Is_No_Value_Fit_The_Filter_Given_If_Doesnt_Exist_In_DbSet()
        {
            List<MockEntity> mockDbSetSource = new()
            {
                new MockEntity { Id = 1 },
                new MockEntity { Id = 2 }
            };
            MockRepository eFRepository = CreateMockRepositoryWithSourceList(mockDbSetSource);

            List<MockEntity> entities = eFRepository.GetList(entity => entity.Id > 2).ToList();

            Assert.Empty(entities);
        }
        
        [Fact]
        public void Add_Throws_ArgumentNullException_If_Given_Entity_Is_Null()
        {
            MockRepository eFRepository = CreateMockRepositoryWithSourceList(new List<MockEntity>());

            Assert.Throws<ArgumentNullException>(() => eFRepository.Add(null));
        }

        [Fact]
        public void Add_Adds_Given_Entity_To_DbSet()
        {
            List<MockEntity> mockDbSetSource = new()
            {
                new MockEntity { Id = 1 },
            };
            MockRepository eFRepository = CreateMockRepositoryWithSourceList(mockDbSetSource);
            MockEntity newEntity = new() { Id = 2 };

            eFRepository.Add(newEntity);

            Assert.Contains(newEntity, mockDbSetSource);
        }

        [Fact]
        public void Update_Throws_ArgumentNullException_If_Given_Entity_Is_Null()
        {
            MockRepository eFRepository = CreateMockRepositoryWithSourceList(new List<MockEntity>());

            Assert.Throws<ArgumentNullException>(() => eFRepository.Update(null));
        }

        [Fact]
        public void Update_Calls_DbSet_Update_To_Update_Entity()
        {
            var dbSet = new Mock<DbSet<MockEntity>>();
            dbSet.Setup(d => d.Update(It.IsAny<MockEntity>()));
            mockDbContext.Setup(ctx => ctx.Set<MockEntity>())
                .Returns(dbSet.Object);
            MockRepository eFRepository = new(mockDbContext.Object);
            var entity = new MockEntity { Id = 1 };

            eFRepository.Update(entity);

            dbSet.Verify(d => d.Update(entity), Times.Once);
        }

        [Fact]
        public void Remove_Throws_ArgumentNullException_If_Given_Entity_Is_Null()
        {
            MockRepository eFRepository = CreateMockRepositoryWithSourceList(new List<MockEntity>());

            Assert.Throws<ArgumentNullException>(() => eFRepository.Remove(null));
        }

        [Fact]
        public void Remove_Removes_Given_Entity_From_DbSet()
        {
            var entity = new MockEntity { Id = 1 };
            List<MockEntity> mockDbSetSource = new()
            {
                entity
            };
            MockRepository eFRepository = CreateMockRepositoryWithSourceList(mockDbSetSource);

            eFRepository.Remove(entity);

            Assert.DoesNotContain(entity, mockDbSetSource);
        }
    }
}
