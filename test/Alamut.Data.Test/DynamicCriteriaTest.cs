using System.Linq;
using Alamut.Data.Paging;
using Alamut.Data.NoSql;
using Alamut.Data.Test.Database;
using Xunit;

namespace Alamut.Data.Test
{
    public class DynamicCriteriaTest
    {
        private readonly AppDbContext _dbContext;

        public DynamicCriteriaTest()
        {
            _dbContext = DbHelper.GetInMemoryInstance();
        }
        
        [Fact]
        public async void DynamicCriteriaExtensions_GetSortedItems_ReturnSortedResult()
        {
            // arrange
            DbHelper.CleanBlog(_dbContext);
            DbHelper.SeedBulkBlogs(_dbContext);
            var expected = _dbContext.Blogs.OrderByDescending(s => s.Id).ToList();

            // act
            var actual = _dbContext.Blogs.Sort("Id desc").ToList();

            // assert
            Assert.Equal(expected.First(), actual.First());
        }
        
        [Fact]
        public async void DynamicCriteriaExtensions_FilterItems_ReturnFilteredResult()
        {
            // arrange
            DbHelper.CleanBlog(_dbContext);
            DbHelper.SeedBulkBlogs(_dbContext);
            var expected = _dbContext.Blogs.FirstOrDefault(q => q.Id == 1);

            // act
            var actual = _dbContext.Blogs.Filter("Id == @0", 1).FirstOrDefault();

            // assert
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async void DynamicCriteriaExtensions_ApplyCriteria_ReturnFilteredResult()
        {
            // arrange
            DbHelper.CleanBlog(_dbContext);
            DbHelper.SeedBulkBlogs(_dbContext);
            var expected = _dbContext.Blogs
                .OrderByDescending(o => o.Id)
                .Where(q => q.Id > 10)
                .ToList();

            // act
            var actual = _dbContext.Blogs.ApplyCriteria(new DynamicCriteria
            {
                Sorts = "Id desc",
                FilterClause = "Id > @0",
                FilterParameters = new object[] {10}
            }).ToList();
                

            // assert
            Assert.Equal(expected, actual);
        }
    }
}