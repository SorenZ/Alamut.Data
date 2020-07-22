using System;
using System.Linq;
using Alamut.Data.Paging;
using Alamut.Data.Test.Database;
using Xunit;

namespace Alamut.Data.Test
{
    public class PaginationTest
    {
        private readonly AppDbContext _dbContext;

        public PaginationTest()
        {
            _dbContext = DbHelper.GetInMemoryInstance();
        }

        [Fact]
        public async void PaginatedExtensions_GetPaginatedAsync_ReturnPaginatedResult()
        {
            // arrange
            DbHelper.CleanBlog(_dbContext);
            var blogList = DbHelper.SeedBulkBlogs(_dbContext);
            var expected = new Paginated<Blog>(blogList.Skip(10).Take(10).ToList(), blogList.Count, 2, 10);

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
