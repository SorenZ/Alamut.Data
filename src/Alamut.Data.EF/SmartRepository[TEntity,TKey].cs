using System.Collections.Generic;
using System.Linq;
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
        /// <inheritdoc />
        public SmartRepository(DbContext dbContext, IMapper mapper) : base(dbContext,mapper)
        { }

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
    }
}
