using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Alamut.Data.Entity;
using Alamut.Data.Paging;
using Alamut.Data.Repository;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF
{
    public class SmartRepository<TEntity, TKey> : Repository<TEntity,TKey>, 
        ISmartRepository<TEntity,TKey> where TEntity : class, IEntity<TKey>, new()

    {
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public SmartRepository(DbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc />
        public async Task<TDto> GetById<TDto>(TKey id, CancellationToken cancellationToken = default) =>
            await Queryable
                .Where(q => q.Id.Equals(id))
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<TDto> Get<TDto>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => 
            await Queryable
                .Where(predicate)
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<List<TDto>> GetAll<TDto>(CancellationToken cancellationToken = default) =>
            await Queryable
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<List<TDto>> GetMany<TDto>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
            await Queryable
                .Where(predicate)
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<List<TDto>> GetByIds<TDto>(IEnumerable<TKey> ids, CancellationToken cancellationToken = default) =>
            await Queryable
                .Where(q => ids.Contains(q.Id))
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<IPaginated<TDto>> GetPaginated<TDto>(IPaginatedCriteria criteria = null, CancellationToken cancellationToken = default) =>
            await Queryable
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToPaginatedAsync(criteria ?? new PaginatedCriteria(), cancellationToken);

        /// <inheritdoc />
        public TEntity Add<TDto>(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            base.Add(entity);
            return entity;
        }

        /// <inheritdoc />
        public async Task<TEntity> Update<TDto>(TKey id, TDto dto, CancellationToken cancellationToken = default)
        {
            var entity = (await base.GetById(id, cancellationToken)) 
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");;

            var updatedEntity = _mapper.Map(dto, entity);
            base.Update(entity);
            return entity;
        }
    }
}
