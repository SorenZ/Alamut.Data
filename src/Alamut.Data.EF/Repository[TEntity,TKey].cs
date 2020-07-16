using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Alamut.Data.Entity;
using Alamut.Data.Repository;
using Alamut.Helpers.Linq;

using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF
{
    public class Repository<TEntity, TKey> : Repository<TEntity>, 
        IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        public Repository(DbContext dbContext) : base(dbContext)
        { }

        public virtual async Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default) =>
            await DbSet.FirstOrDefaultAsync(q => q.Id.Equals(id), cancellationToken);

        public virtual async Task<List<TEntity>> GetByIds(IEnumerable<TKey> ids,
            CancellationToken cancellationToken = default) =>
            await DbSet.Where(q => ids.Contains(q.Id)).ToListAsync(cancellationToken);
        
        public virtual async Task UpdateFieldById<TField>(TKey id,
            Expression<Func<TEntity, TField>> memberExpression, TField value)
        {
            var entity = (await DbSet.FirstOrDefaultAsync(q => q.Id.Equals(id)))
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");

            var memberName = LambdaExpressions.GetName(memberExpression);

            entity.GetType()
                .GetProperty(memberName)
                ?.SetValue(entity, value);
        }

        public virtual void GenericUpdate(TKey id, Dictionary<string, object> fieldset)
        {
            var entity = DbSet.FirstOrDefault(q => q.Id.Equals(id))
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");

            foreach (var field in fieldset)
            {
                entity.GetType()
                    .GetProperty(field.Key)?
                    .SetValue(entity, field.Value);
            }
        }

        public virtual async Task DeleteById(TKey id, CancellationToken cancellationToken)
        {
            var entity = (await DbSet.FirstOrDefaultAsync(q => q.Id.Equals(id), cancellationToken))
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");

            DbSet.Remove(entity);
        }
    }
}