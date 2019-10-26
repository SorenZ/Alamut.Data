using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Alamut.Data.Paging;
using Alamut.Data.Repository;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF
{
    public class SmartRepository<TEntity> : Repository<TEntity>,
        ISmartRepository<TEntity> where TEntity : class
    {
        protected readonly IMapper Mapper;

        /// <inheritdoc />
        public SmartRepository(DbContext dbContext, IMapper mapper) : base(dbContext)
        {
            Mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<TDto> Get<TDto>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)=> 
            await Queryable
                .Where(predicate)
                .ProjectTo<TDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<List<TDto>> GetAll<TDto>(CancellationToken cancellationToken = default) =>
            await Queryable
                .ProjectTo<TDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<List<TDto>> GetMany<TDto>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
            await Queryable
                .Where(predicate)
                .ProjectTo<TDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<IPaginated<TDto>> GetPaginated<TDto>(IPaginatedCriteria criteria = null, 
            CancellationToken cancellationToken = default)=>
            await Queryable
                .ProjectTo<TDto>(Mapper.ConfigurationProvider)
                .ToPaginatedAsync(criteria ?? new PaginatedCriteria(), cancellationToken);

        /// <inheritdoc />
        public TEntity Add<TDto>(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            base.Add(entity);
            return entity;
        }

        /// <inheritdoc />
        public async Task<TEntity> UpdateById<TDto>(object id, TDto dto, CancellationToken cancellationToken = default) => 
            await UpdateById(new[] {id}, dto, cancellationToken);

        /// <inheritdoc />
        public async Task<TEntity> UpdateById<TDto>(object[] ids, TDto dto, CancellationToken cancellationToken = default)
        {
            var entity = (await DbSet.FindAsync(ids, cancellationToken: cancellationToken)) 
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {ids}");

            var updatedEntity = Mapper.Map(dto, entity);
            base.Update(entity);
            return updatedEntity;
        }
    }
}
