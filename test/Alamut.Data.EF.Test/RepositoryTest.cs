using System;
using Alamut.Data.EF.Test.Database;
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
        public async void Repository_GetByIdAsync_ReturnEntity()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var expected = DbHelper.Seed_SingleBlog(_dbContext);

            // act
            var actual = await repository.GetByIdAsync(expected.Id);

            // assert
            Assert.Equal(expected, actual);
        }
    }
}
