// ReSharper disable RedundantArgumentDefaultValue

using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            var expected = DbHelper.SeedSingleBlog(_dbContext);

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
            var expected = DbHelper.SeedSingleBlog(_dbContext);

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
            DbHelper.SeedSingleBlog(_dbContext);

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
            var entity1 = DbHelper.SeedSingleBlog(_dbContext);
            var entity2 = DbHelper.SeedSingleBlog(_dbContext);
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

        [Fact]
        public void Repository_AddEntity_EntityAdded()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            DbHelper.CleanBlog(_dbContext);
            var expected = new Blog
            {
                Url = "https://github.com/SorenZ/Alamut.DotNet",
                Rating = 5
            };

            // act
            repository.Add(expected);
            var entry = _dbContext.Entry(expected);

            // assert
            Assert.True(entry.State == EntityState.Added);
        }


        [Fact]
        public async void Repository_AddEntityAndCommit_EntityAddedToDatabase()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            DbHelper.CleanBlog(_dbContext);
            var expected = new Blog
            {
                Url = "https://github.com/SorenZ/Alamut.DotNet",
                Rating = 5
            };

            // act
            repository.Add(expected);
            await repository.CommitAsync(CancellationToken.None);

            // assert
            Assert.Contains(expected, _dbContext.Blogs);
        }

        [Fact]
        public async void Repository_AddRangeAndCommit_EntitiesAddedToDatabase()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            DbHelper.CleanBlog(_dbContext);
            var expected = new[]
            {
                new Blog
                {
                    Url = "https://github.com/SorenZ/Alamut.Data",
                    Rating = 5
                },
                new Blog
                {
                    Url = "https://github.com/SorenZ/Alamut.AspNet",
                    Rating = 4
                }
            };

            // act
            repository.AddRange(expected);
            await repository.CommitAsync(CancellationToken.None);

            // assert
            Assert.Contains(expected.First(), _dbContext.Blogs);
            Assert.Contains(expected.Last(), _dbContext.Blogs);
        }

        [Fact]
        public void Repository_UpdateEntity_EntityUpdated()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var expected = DbHelper.SeedSingleBlog(_dbContext);

            // act
            expected.Rating = 10;
            repository.Update(expected);

            var entry = _dbContext.Entry(expected);

            // assert
            Assert.True(entry.State == EntityState.Modified);
            Assert.Contains(expected, _dbContext.Blogs);
        }

        [Fact]
        public void Repository_UpdateUnAttachedEntity_EntityUpdated()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var entity = DbHelper.SeedSingleBlog(_dbContext);
            _dbContext.Entry(entity).State = EntityState.Detached;
            
            var expected = new Blog
            {
                Id = entity.Id,
                Rating = 10,
                Posts = null,
                Url = entity.Url
            };

            // act
            repository.Update(expected);

            // assert
            Assert.Contains(expected, _dbContext.Blogs);
        }

        [Fact]
        public void Repository_UpdateFieldById_EntityUpdated()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            DbHelper.CleanBlog(_dbContext);
            var expected = DbHelper.SeedSingleBlog(_dbContext);

            // act
            repository.UpdateFieldById(expected.Id, blog => blog.Rating, 100);
            
            var entry = _dbContext.Entry(expected);

            // assert
            Assert.True(entry.State == EntityState.Modified);
            Assert.Equal(expected.Rating, _dbContext.Blogs.First().Rating);
        }

        [Fact]
        public void Repository_UpdateField_EntityUpdated()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            DbHelper.CleanBlog(_dbContext);
            var expected = DbHelper.SeedSingleBlog(_dbContext);

            // act
            repository.UpdateField(blog => blog.Id == expected.Id, blog => blog.Rating, 100);
            
            var entry = _dbContext.Entry(expected);

            // assert
            Assert.True(entry.State == EntityState.Modified);
            Assert.Equal(expected.Rating, _dbContext.Blogs.First().Rating);
        }

        [Fact]
        public void Repository_GenericUpdate_EntityUpdated()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            DbHelper.CleanBlog(_dbContext);
            var expected = DbHelper.SeedSingleBlog(_dbContext);
            var fieldset = new Dictionary<string, object>
            {
                {"Rating", 100},
                {"Url", "test"}
            };
            

            // act
            repository.GenericUpdate(expected.Id, fieldset);
            
            var entry = _dbContext.Entry(expected);
            var actual = repository.GetById(expected.Id).Result;

            // assert
            Assert.True(entry.State == EntityState.Modified);
            Assert.Equal(100, actual.Rating);
            Assert.Equal("test", actual.Url);
        }

        [Fact]
        public void Repository_DeleteById_EntityDeleted()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var expected = DbHelper.SeedSingleBlog(_dbContext);

            // act
            repository.DeleteById(expected.Id);

            var entry = _dbContext.Entry(expected);

            // assert
            Assert.True(entry.State == EntityState.Deleted);
            //Assert.DoesNotContain(expected, _dbContext.Blogs);
        }

        [Fact]
        public async void Repository_DeleteByIdAndCommit_EntityDeletedInDatabase()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var expected = DbHelper.SeedSingleBlog(_dbContext);

            // act
            repository.DeleteById(expected.Id);
            var result = await repository.CommitAsync(CancellationToken.None);

            // assert
            Assert.DoesNotContain(expected, _dbContext.Blogs);
            Assert.True(result.Succeed);
        }

        [Fact]
        public async void Repository_DeleteUnAttachedEntity_EntityDeletedInDatabase()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            var expected = DbHelper.SeedSingleBlog(_dbContext);
            _dbContext.Entry(expected).State = EntityState.Detached;

            // act
            repository.Delete(new Blog() {Id = expected.Id});
            var result = await repository.CommitAsync(CancellationToken.None);

            // assert
            Assert.DoesNotContain(expected, _dbContext.Blogs);
            Assert.True(result.Succeed);
        }

        [Fact]
        public async void Repository_DeleteManyEntities_EntitiesDeletedInDatabase()
        {
            // arrange
            var repository = new Repository<Blog,int>(_dbContext);
            DbHelper.CleanBlog(_dbContext);
            var entities = DbHelper.SeedBulkBlogs(_dbContext);

            // act
            repository.DeleteMany(d => true);
            var result = await repository.CommitAsync(CancellationToken.None);

            // assert
            Assert.Empty(_dbContext.Blogs);
            Assert.True(result.Succeed);
        }
    }
}
