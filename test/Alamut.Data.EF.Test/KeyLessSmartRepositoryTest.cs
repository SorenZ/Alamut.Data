using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alamut.Data.EF.Test.Database;
using Alamut.Data.EF.Test.Models;
using Alamut.Data.Paging;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alamut.Data.EF.Test
{
    public class KeyLessSmartRepositoryTest
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public KeyLessSmartRepositoryTest()
        {
            _dbContext = DbHelper.GetInMemoryInstance();

            var configuration = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<Blog, BlogViewModel>().ReverseMap();
                //cfg.CreateMap<Bar, BarDto>();
            });
            
            configuration.AssertConfigurationIsValid();

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async void SmartRepository_Get_ReturnDTO()
        {
            // arrange
            var repository = new SmartRepository<Blog>(_dbContext, _mapper);
            var entity = DbHelper.SeedSingleBlog(_dbContext);
            var expected = _mapper.Map<BlogViewModel>(entity);

            // act
            var actual = await repository.Get<BlogViewModel>(q => q.Rating == entity.Rating);

            // assert
            Assert.Equal(expected, actual, new BlogDtoIEqualityComparer());
        }

        [Fact]
        public async void SmartRepository_GetAll_ReturnDTOs()
        {
            // arrange
            var repository = new SmartRepository<Blog>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var entities = DbHelper.SeedBulkBlogs(_dbContext);
            var expected = _mapper.Map<List<BlogViewModel>>(entities);

            // act
            var actual = await repository.GetAll<BlogViewModel>();

            // assert
            Assert.Equal(expected, actual, new BlogDtoIEqualityComparer());
        }

        [Fact]
        public async void SmartRepository_GetMany_ReturnDTOs()
        {
            // arrange
            var repository = new SmartRepository<Blog>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var entities = DbHelper.SeedBulkBlogs(_dbContext);
            var expected = _mapper.Map<List<BlogViewModel>>(entities);

            // act
            var actual = await repository.GetMany<BlogViewModel>(q => true);

            // assert
            Assert.Equal(expected, actual, new BlogDtoIEqualityComparer());
        }

        [Fact]
        public async void Repository_GetPaginatedWithDefaultCriteria_ReturnDefaultPaginated()
        {
            // arrange
            var repository = new SmartRepository<Blog>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var blogs = DbHelper.SeedBulkBlogs(_dbContext);
            var dtoList = _mapper.Map<List<BlogViewModel>>(blogs);
            var expected = new Paginated<BlogViewModel>(dtoList.Take(10).ToList(), blogs.Count, 1, 10);

            // act
            var actual = await repository.GetPaginated<BlogViewModel>();

            // assert
            Assert.Equal(expected, actual, new PaginatedEqualityComparer<BlogViewModel>());
        }

        [Fact]
        public async void Repository_GetPaginatedWithCustomizedCriteria_ReturnExpectedPaginated()
        {
            // arrange
            var repository = new SmartRepository<Blog>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            DbHelper.CleanBlog(_dbContext);
            var blogs = DbHelper.SeedBulkBlogs(_dbContext);
            var dtoList = _mapper.Map<List<BlogViewModel>>(blogs);
            var expected = new Paginated<BlogViewModel>(dtoList.Skip(10).Take(10).ToList(), blogs.Count, 2, 10);

            // act
            var actual = await repository.GetPaginated<BlogViewModel>(new PaginatedCriteria(2, 10));

            // assert
            Assert.Equal(expected, actual, new PaginatedEqualityComparer<BlogViewModel>());
        }

        [Fact]
        public void Repository_AddDto_EntityAdded()
        {
            // arrange
            var repository = new SmartRepository<Blog>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var expected = new BlogViewModel
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
            var repository = new SmartRepository<Blog>(_dbContext, _mapper);
            DbHelper.CleanBlog(_dbContext);
            var entity = DbHelper.SeedSingleBlog(_dbContext);
            _dbContext.Entry(entity).State = EntityState.Detached;

            var expected = new BlogViewModel
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
