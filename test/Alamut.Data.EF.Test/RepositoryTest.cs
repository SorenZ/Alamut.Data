using System;
using System.Linq;
using Alamut.Data.EF.Test.Database;
using Alamut.Data.Paging;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alamut.Data.EF.Test
{
    public class RepositoryTest
    {
        private readonly AppDbContext _dbContext;

        public RepositoryTest()
        {
            _dbContext = DbHelper.GetInMemoryInstance();
        }

        [Fact]
        public void Repository_Queryable_ReturnQuery()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var expected = _dbContext.Blogs.AsQueryable();

            // act
            var actual = repository.Queryable;

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Repository_Queryable_ContainsSeededEntities()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var expected = DbHelper.Seed_SingleBlog(_dbContext);

            // act
            var actual = repository.Queryable;

            // assert
            Assert.Contains(expected, actual);
        }

        [Fact]
        public async void Repository_GetById_ReturnEntity()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var expected = DbHelper.Seed_SingleBlog(_dbContext);

            // act
            var actual = await repository.GetById(expected.Id);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void Repository_GetById_ReturnNull()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            DbHelper.Seed_SingleBlog(_dbContext);

            // act
            var actual = await repository.GetById(0);

            // assert
            Assert.Null(actual);
        }

        [Fact]
        public async void Repository_GetByIds_ReturnEntities()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var entity1 = DbHelper.Seed_SingleBlog(_dbContext);
            var entity2 = DbHelper.Seed_SingleBlog(_dbContext);
            var ids = new[] {entity1.Id, entity2.Id};


            // act
            var actual = await repository.GetByIds(ids);

            // assert
            Assert.Contains(entity1, actual);
            Assert.Contains(entity2, actual);
            Assert.Equal(2, actual.Count);
        }

        [Fact]
        public async void Repository_GetPaginatedWithDefaultCriteria_ReturnDefaultPaginated()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            DbHelper.CleanBlog(_dbContext);
            var blogs = DbHelper.SeedBulkBlogs(_dbContext);
            var expected = new Paginated<Blog>(blogs.Take(10).ToList(), blogs.Count, 1, 10);

            // act
            var actual = await repository.GetPaginated();

            // assert
            Assert.Equal(expected, actual, new PaginatedEqualityComparer<Blog>());
        }

        [Fact]
        public async void Repository_GetPaginatedWithCustomizedCriteria_ReturnExpectedPaginated()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            DbHelper.CleanBlog(_dbContext);
            var blogs = DbHelper.SeedBulkBlogs(_dbContext);
            var expected = new Paginated<Blog>(blogs.Skip(10).Take(10).ToList(), blogs.Count, 2, 10);

            // act
            var actual = await repository.GetPaginated(new PaginatedCriteria(2, 10));

            // assert
            Assert.Equal(expected, actual, new PaginatedEqualityComparer<Blog>());
        }

        
    }
}
