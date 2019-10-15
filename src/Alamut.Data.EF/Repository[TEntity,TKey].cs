using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Alamut.Abstractions.Structure;
using Alamut.Data.Entity;
using Alamut.Data.Paging;
using Alamut.Data.Repository;
using Alamut.Helpers.Linq;

using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF
{
    /// <inheritdoc />
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet;

        public virtual IQueryable<TEntity> Queryable => DbSet;

        public virtual async Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default) =>
            await DbSet.FirstOrDefaultAsync(q => q.Id.Equals(id), cancellationToken);

        public virtual async Task<List<TEntity>> GetByIds(IEnumerable<TKey> ids,
            CancellationToken cancellationToken = default) =>
            await DbSet.Where(q => ids.Contains(q.Id)).ToListAsync(cancellationToken);

        public virtual async Task<IPaginated<TEntity>> GetPaginated(IPaginatedCriteria criteria = null,
            CancellationToken cancellationToken = default) =>
            await DbSet.ToPaginatedAsync(criteria ?? new PaginatedCriteria(), cancellationToken);

        public virtual void Add(TEntity entity) => DbSet.Add(entity);
        
        public virtual void AddRange(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);

        public virtual void Update(TEntity entity)
        {
            var entry = _dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            { DbSet.Attach(entity); }

            entry.State = EntityState.Modified;
        }
        
        public virtual void UpdateFieldById<TField>(TKey id,
            Expression<Func<TEntity, TField>> memberExpression, TField value)
        {
            var entity = DbSet.FirstOrDefault(q => q.Id.Equals(id))
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");

            var memberName = LambdaExpressions.GetName(memberExpression);

            entity.GetType()
                .GetProperty(memberName)
                ?.SetValue(entity, value);
        }

        public virtual void UpdateField<TField>(Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TField>> memberExpression,
            TField value)
        {
            var entity = DbSet.FirstOrDefault(filterExpression)
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with query : {filterExpression}");

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

        public virtual void DeleteById(TKey id)
        {
            var entity = DbSet.FirstOrDefault(q => q.Id.Equals(id))
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");

            DbSet.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            var entry = _dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            { DbSet.Attach(entity); }

            entry.State = EntityState.Deleted;
        }

        public virtual void DeleteMany(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = DbSet.Where(predicate);
            DbSet.RemoveRange(entities);
        }

        public virtual Task<Result> CommitAsync(CancellationToken cancellationToken) => 
            _dbContext.SaveChangeAndReturnResult(cancellationToken);
    }
}