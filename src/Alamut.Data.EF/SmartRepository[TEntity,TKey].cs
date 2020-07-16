using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Alamut.Data.Entity;
using Alamut.Data.Repository;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF
{
    public class SmartRepository<TEntity, TKey> : SmartRepository<TEntity>, 
        ISmartRepository<TEntity,TKey> where TEntity : class, IEntity<TKey>, new()

    {
        private readonly IRepository<TEntity, TKey> _internalRepository;

        /// <inheritdoc />
        public SmartRepository(DbContext dbContext, IMapper mapper, IRepository<TEntity, TKey> internalRepository) : base(dbContext,mapper)
        {
            _internalRepository = internalRepository;
        }

        /// <inheritdoc />
        public async Task<TDto> GetById<TDto>(TKey id, CancellationToken cancellationToken = default) =>
            await Queryable
                .Where(q => q.Id.Equals(id))
                .ProjectTo<TDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<List<TDto>> GetByIds<TDto>(IEnumerable<TKey> ids, CancellationToken cancellationToken = default) =>
            await Queryable
                .Where(q => ids.Contains(q.Id))
                .ProjectTo<TDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<TEntity> UpdateById<TDto>(TKey id, TDto dto, CancellationToken cancellationToken = default)
        {
            var entity = (await DbSet.FirstOrDefaultAsync(q => q.Id.Equals(id), cancellationToken)) 
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");;

            var updatedEntity = Mapper.Map(dto, entity);
            base.Update(entity);
            return updatedEntity;
        }

        public async Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default) =>
            await _internalRepository.GetById(id, cancellationToken);

        public async Task<List<TEntity>> GetByIds(IEnumerable<TKey> ids, CancellationToken cancellationToken = default) => 
            await _internalRepository.GetByIds(ids, cancellationToken);

        public async Task UpdateFieldById<TField>(TKey id, Expression<Func<TEntity, TField>> memberExpression, TField value) => 
            await _internalRepository.UpdateFieldById(id, memberExpression, value);

        public void GenericUpdate(TKey id, Dictionary<string, object> fieldset) => 
            _internalRepository.GenericUpdate(id, fieldset);

        public async Task DeleteById(TKey id, CancellationToken cancellationToken) =>
            await _internalRepository.DeleteById(id, cancellationToken);
    }
}
