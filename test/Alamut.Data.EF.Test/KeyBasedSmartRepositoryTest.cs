using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alamut.Data.EF.Test.Database;
using Alamut.Data.EF.Test.Models;
using Alamut.Data.Paging;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alamut.Data.EF.Test
{
    public class KeyBasedSmartRepositoryTest
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public KeyBasedSmartRepositoryTest()
        {
            _dbContext = DbHelper.GetInMemoryInstance();

            var configuration = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<Blog, BlogDto>().ReverseMap();
                //cfg.CreateMap<Bar, BarDto>();
            });
            
            configuration.AssertConfigurationIsValid();

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task SmartRepository_GetById_ReturnDTO()
        {
            // arrange
            var repository = new SmartRepository<Blog,int>(_dbContext, _mapper);
            var entity = DbHelper.SeedSingleBlog(_dbContext);
            var expected = _mapper.Map<BlogDto>(entity);

            // act
            var actual = await repository.GetById<BlogDto>(entity.Id);

            // assert
            Assert.Equal(expected, actual, new BlogDtoIEqualityComparer());
        }

        [Fact]
        public async Task SmartRepository_Get_ReturnDTO()
        {
            // arrange
            var repository = new SmartRepository<Blog,int>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var entity = DbHelper.SeedSingleBlog(_dbContext);
            var expected = _mapper.Map<BlogDto>(entity);

            // act
            var actual = await repository.Get<BlogDto>(q => q.Rating == entity.Rating);

            // assert
            Assert.Equal(expected, actual, new BlogDtoIEqualityComparer());
        }

        [Fact]
        public async Task SmartRepository_GetAll_ReturnDTOs()
        {
            // arrange
            var repository = new SmartRepository<Blog,int>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var entities = DbHelper.SeedBulkBlogs(_dbContext);
            var expected = _mapper.Map<List<BlogDto>>(entities);

            // act
            var actual = await repository.GetAll<BlogDto>();

            // assert
            Assert.Equal(expected, actual, new BlogDtoIEqualityComparer());
        }

        [Fact]
        public async Task SmartRepository_GetMany_ReturnDTOs()
        {
            // arrange
            var repository = new SmartRepository<Blog,int>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var entities = DbHelper.SeedBulkBlogs(_dbContext);
            var expected = _mapper.Map<List<BlogDto>>(entities);

            // act
            var actual = await repository.GetMany<BlogDto>(q => true);

            // assert
            Assert.Equal(expected, actual, new BlogDtoIEqualityComparer());
        }

        [Fact]
        public async void SmartRepository_GetByIds_ReturnDTOs()
        {
            // arrange
            var repository = new SmartRepository<Blog,int>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var entity1 = DbHelper.SeedSingleBlog(_dbContext);
            var entity2 = DbHelper.SeedSingleBlog(_dbContext);
            var ids = new[] {entity1.Id, entity2.Id};

            var expected = _mapper.Map<List<BlogDto>>(new[] {entity1, entity2})
                .OrderBy(o => o.Id);

            // act
            var actual = (await repository.GetByIds<BlogDto>(ids))
                .OrderBy(o => o.Id);
                

            // assert
            Assert.Equal(expected, actual, new BlogDtoIEqualityComparer());
        }

        [Fact]
        public async void Repository_GetPaginatedWithDefaultCriteria_ReturnDefaultPaginated()
        {
            // arrange
            var repository = new SmartRepository<Blog,int>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var blogs = DbHelper.SeedBulkBlogs(_dbContext);
            var dtoList = _mapper.Map<List<BlogDto>>(blogs);
            var expected = new Paginated<BlogDto>(dtoList.Take(10).ToList(), blogs.Count, 1, 10);

            // act
            var actual = await repository.GetPaginated<BlogDto>();

            // assert
            Assert.Equal(expected, actual, new PaginatedEqualityComparer<BlogDto>());
        }

        [Fact]
        public async Task Repository_GetPaginatedWithCustomizedCriteria_ReturnExpectedPaginated()
        {
            // arrange
            var repository = new SmartRepository<Blog,int>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var blogs = DbHelper.SeedBulkBlogs(_dbContext);
            var dtoList = _mapper.Map<List<BlogDto>>(blogs);
            var expected = new Paginated<BlogDto>(dtoList.Skip(10).Take(10).ToList(), blogs.Count, 2, 10);

            // act
            var actual = await repository.GetPaginated<BlogDto>(new PaginatedCriteria(2, 10));

            // assert
            Assert.Equal(expected, actual, new PaginatedEqualityComparer<BlogDto>());
        }

        [Fact]
        public void Repository_AddDto_EntityAdded()
        {
            // arrange
            var repository = new SmartRepository<Blog,int>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var expected = new BlogDto
            {
                Rating = 5
            };

            // act
            var actual = repository.Add(expected);
            var entry = _dbContext.Entry(actual);

            // assert
            Assert.True(entry.State == EntityState.Added);
        }

        [Fact]
        public async void Repository_UpdateEntity_EntityUpdated()
        {
            // arrange
            var repository = new SmartRepository<Blog,int>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var entity = DbHelper.SeedSingleBlog(_dbContext);
            _dbContext.Entry(entity).State = EntityState.Detached;

            var expected = new BlogDto
            {
                Id = entity.Id,
                Rating = 10
            };
            
            // act
            var updatedEntity = await repository.UpdateById(entity.Id, expected);
            var entry = _dbContext.Entry(updatedEntity);

            // assert
            Assert.True(entry.State == EntityState.Modified);
            Assert.Equal(entity.Url, updatedEntity.Url);
            Assert.Contains(updatedEntity, _dbContext.Blogs);
        }
    }
}
