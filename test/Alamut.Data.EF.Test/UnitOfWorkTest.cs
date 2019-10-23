using System.Threading;

using Alamut.Data.EF.Test.Database;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Alamut.Data.EF.Test
{
    public class UnitOfWorkTest
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWorkTest()
        {
            _dbContext = DbHelper.GetInMemoryInstance();
        }

        [Fact]
        public void UnitOfWork_Commit_SaveEntitiesToDatabase()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var uow = new UnitOfWork(_dbContext);
            var expected = new Blog
            {
                Url = "https://github.com/SorenZ/Alamut.DotNet",
                Rating = 5
            };

            // act
            repository.Add(expected);
            var result = uow.Commit();

            // assert
            Assert.True(result);
            Assert.Contains(expected, _dbContext.Blogs);
        }

        [Fact]
        public async void UnitOfWork_CommitAsync_SaveEntitiesToDatabase()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var uow = new UnitOfWork(_dbContext);
            var expected = new Blog
            {
                Url = "https://github.com/SorenZ/Alamut.DotNet",
                Rating = 5
            };

            // act
            repository.Add(expected);
            var result = await uow.CommitAsync(CancellationToken.None);

            // assert
            Assert.True(result);
            Assert.Contains(expected, _dbContext.Blogs);
        }

        [Fact]
        public void UnitOfWork_RejectChangesOfAddedEntity_UnTrackAddedEntity()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var uow = new UnitOfWork(_dbContext);
            var expected = new Blog
            {
                Url = "https://github.com/SorenZ/Alamut.DotNet",
                Rating = 5
            };

            // act
            repository.Add(expected);
            uow.RejectChanges();
            var entry = _dbContext.Entry(expected);

            // assert
            Assert.Equal(EntityState.Detached, entry.State);
            Assert.DoesNotContain(expected, _dbContext.Blogs);
        }

        [Fact]
        public void UnitOfWork_RejectChangesOfDeletedEntity_EntityReturnedBack()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var uow = new UnitOfWork(_dbContext);
            var expected = DbHelper.SeedSingleBlog(_dbContext);

            // act
            repository.Delete(expected);
            uow.RejectChanges();
            var entry = _dbContext.Entry(expected);

            // assert
            Assert.Equal(EntityState.Unchanged, entry.State);
            Assert.Contains(expected, _dbContext.Blogs);
        }

        [Fact]
        public void UnitOfWork_RejectChangesOfModifiedEntity_EntityRetrievesLastState()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var uow = new UnitOfWork(_dbContext);
            var expected = DbHelper.SeedSingleBlog(_dbContext);

            // act
            expected.Rating = 100_000;
            repository.Update(expected);
            uow.RejectChanges();
            var entry = _dbContext.Entry(expected);
            var actual = repository.GetById(expected.Id).Result;

            // assert
            Assert.Equal(EntityState.Unchanged, entry.State);
            Assert.NotEqual(100_000, actual.Rating);
        }
    }
}
