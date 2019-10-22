using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alamut.Abstractions.Structure;
using Alamut.Data.Paging;
using Alamut.Data.Repository;
using Alamut.Helpers.Linq;
using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet;

        /// <inheritdoc />
        public virtual IQueryable<TEntity> Queryable => DbSet;

        /// <inheritdoc />
        public virtual async Task<TEntity> GetById(object id, CancellationToken cancellationToken = default) =>
            await DbSet.FindAsync(id, cancellationToken);

        /// <inheritdoc />
        public virtual async Task<IPaginated<TEntity>> GetPaginated(IPaginatedCriteria criteria = null,
            CancellationToken cancellationToken = default) =>
            await DbSet.ToPaginatedAsync(criteria ?? new PaginatedCriteria(), cancellationToken);

        /// <inheritdoc />
        public virtual void Add(TEntity entity) => DbSet.Add(entity);

        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);

        /// <inheritdoc />
        public virtual void Update(TEntity entity)
        {
            var entry = _dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            { DbSet.Attach(entity); }

            entry.State = EntityState.Modified;
        }

        /// <inheritdoc />
        public virtual void UpdateFieldById<TField>(object id,
            Expression<Func<TEntity, TField>> memberExpression, TField value)
        {
            var entity = DbSet.Find(id)
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");

            var memberName = LambdaExpressions.GetName(memberExpression);

            entity.GetType()
                .GetProperty(memberName)
                ?.SetValue(entity, value);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public virtual void GenericUpdate(object id, Dictionary<string, object> fieldset)
        {
            var entity = DbSet.Find(id)
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");

            foreach (var field in fieldset)
            {
                entity.GetType()
                    .GetProperty(field.Key)?
                    .SetValue(entity, field.Value);
            }
        }

        /// <inheritdoc />
        public virtual void DeleteById(object id)
        {
            var entity = DbSet.Find(id)
                         ?? throw new KeyNotFoundException(
                             $"there is no item in {typeof(TEntity).Name} with id : {id}");

            DbSet.Remove(entity);
        }

        /// <inheritdoc />
        public void Delete(TEntity entity)
        {
            var entry = _dbContext.Entry(entity);

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
            _dbContext.SaveChangeAndReturnResult(cancellationToken);
    }
}
