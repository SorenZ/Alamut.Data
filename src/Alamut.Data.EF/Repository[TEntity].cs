using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Alamut.Abstractions.Structure;
using Alamut.Data.NoSql;
using Alamut.Data.Paging;
using Alamut.Data.Repository;
using Alamut.Helpers.Linq;

using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;
        
        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        

        /// <inheritdoc />
        public virtual IQueryable<TEntity> Queryable => DbSet;

        /// <inheritdoc />
        public virtual async Task<TEntity> FindById(params object[] ids) =>
            await DbSet.FindAsync(ids);

        /// <inheritdoc />
        public virtual async Task<IPaginated<TEntity>> GetPaginated(IPaginatedCriteria criteria = null,
            CancellationToken cancellationToken = default) =>
            await DbSet.ToPaginatedAsync(criteria ?? new PaginatedCriteria(), cancellationToken);

        public virtual async Task<IPaginated<TEntity>> GetPaginated(DynamicPaginatedCriteria criteria, CancellationToken cancellationToken = default)
        {
            return await DbSet.ApplyCriteria(criteria)
                .ToPaginatedAsync(criteria, cancellationToken);
        }

        /// <inheritdoc />
        public virtual void Add(TEntity entity) => DbSet.Add(entity);

        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);

        /// <inheritdoc />
        public virtual void Update(TEntity entity)
        {
            var entry = DbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            { DbSet.Attach(entity); }

            entry.State = EntityState.Modified;
        }

        /// <inheritdoc />
        public virtual async Task UpdateFieldById<TField>(object id,
            Expression<Func<TEntity, TField>> memberExpression, TField value)
        {
            await UpdateFieldById(new[] {id}, memberExpression, value);
        }

        public virtual async Task UpdateFieldById<TField>(object[] ids,
            Expression<Func<TEntity, TField>> memberExpression, TField value)
        {
            var entity = await DbSet.FindAsync(ids)
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {ids}");

            var memberName = LambdaExpressions.GetName(memberExpression);

            entity.GetType()
                .GetProperty(memberName)
                ?.SetValue(entity, value);
        }

        /// <inheritdoc />
        public virtual async Task UpdateField<TField>(Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TField>> memberExpression,
            TField value)
        {
            var entity = (await DbSet.FirstOrDefaultAsync(filterExpression))
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with query : {filterExpression}");

            var memberName = LambdaExpressions.GetName(memberExpression);
            entity.GetType()
                .GetProperty(memberName)
                ?.SetValue(entity, value);
        }

        /// <inheritdoc />
        public async Task GenericUpdate(object id, Dictionary<string, object> fieldset) => 
            await GenericUpdate(new[] {id}, fieldset);

        /// <inheritdoc />
        public virtual async Task GenericUpdate(object[] ids, Dictionary<string, object> fieldset)
        {
            var entity = await DbSet.FindAsync(ids)
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {ids}");

            foreach (var field in fieldset)
            {
                entity.GetType()
                    .GetProperty(field.Key)?
                    .SetValue(entity, field.Value);
            }
        }



        /// <inheritdoc />
        public virtual async Task DeleteById(params object[] id)
        {
            var entity = await DbSet.FindAsync(id)
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");

            DbSet.Remove(entity);
        }

        /// <inheritdoc />
        public void Delete(TEntity entity)
        {
            var entry = DbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            { DbSet.Attach(entity); }

            entry.State = EntityState.Deleted;
        }

        /// <inheritdoc />
        public virtual void DeleteMany(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = DbSet.Where(predicate);
            DbSet.RemoveRange(entities);
        }

        /// <inheritdoc />
        public virtual Task<Result> CommitAsync(CancellationToken cancellationToken) => 
            DbContext.SaveChangeAndReturnResult(cancellationToken);
    }
}
