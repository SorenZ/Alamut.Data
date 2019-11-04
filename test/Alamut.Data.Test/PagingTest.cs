using System;
using System.Linq;
using Alamut.Data.EF.Test.Database;
using Alamut.Data.Paging;
using Xunit;

namespace Alamut.Data.Test
{
    public class PagingTest
    {
        private readonly AppDbContext _dbContext;

        public PagingTest()
        {
            _dbContext = DbHelper.GetInMemoryInstance();
        }

        [Fact]
        public async void PaginatedExtensions_GetPaginatedAsync_ReturnPaginatedResult()
        {
            // arrange
            DbHelper.CleanBlog(_dbContext);
            var blogs = DbHelper.SeedBulkBlogs(_dbContext);
            var expected = new Paginated<Blog>(blogs.Skip(10).Take(10).ToList(), blogs.Count, 2, 10);

            // act
            var actual = await _dbContext.Blogs.ToPaginatedAsync(new PaginatedCriteria(2, 10));

            // assert
            Assert.Equal(expected, actual, new PaginatedEqualityComparer<Blog>());
        }

        [Fact]
        public void PaginatedExtensions_GetPaginated_ReturnPaginatedResult()
        {
            // arrange
            DbHelper.CleanBlog(_dbContext);
            var blogs = DbHelper.SeedBulkBlogs(_dbContext);
            var expected = new Paginated<Blog>(blogs.Skip(10).Take(10).ToList(), blogs.Count, 2, 10);

            // act
            var actual =  _dbContext.Blogs.ToPaginated(new PaginatedCriteria(2, 10));

            // assert
            Assert.Equal(expected, actual, new PaginatedEqualityComparer<Blog>());
        }
    }
}
