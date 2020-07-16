using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Alamut.Abstractions.Structure;
using Alamut.Data.NoSql;
using Alamut.Data.Paging;

namespace Alamut.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// provides a queryable source of elements
        /// the Queryable has not been tracked in sql repositories
        /// </summary>
        IQueryable<TEntity> Queryable { get; }

        /// <summary>
        /// gets an Entity by id 
        /// </summary>
        /// <param name="ids">entity key(s)</param>
        /// <returns>an Entity or null</returns>
        Task<TEntity> FindById(params object[] ids);

        /// <summary>
        /// gets a list of Entities in Paginated data-type filtered by provided criteria or default 
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPaginated<TEntity>> GetPaginated(IPaginatedCriteria criteria = null, CancellationToken cancellationToken = default);

        Task<IPaginated<TEntity>> GetPaginated(DynamicPaginatedCriteria criteria, CancellationToken cancellationToken = default);

        /// <summary>
        /// adds an Entity to the current context
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// adds a list of Entities to the current Context
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// updates an Entity in the current Context
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// updates an item (one field) by expression member selector filter by id
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="id">the key</param>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        /// <remarks>
        /// Even if multiple documents match the filter, only one will be updated because we used UpdateOne
        /// </remarks>
        Task UpdateFieldById<TField>(object id,
            Expression<Func<TEntity, TField>> memberExpression,
            TField value);

        /// <summary>
        /// updates an item (on field) by expression member selector filter by ids
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="ids">keys</param>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        Task UpdateFieldById<TField>(object[] ids,
            Expression<Func<TEntity, TField>> memberExpression, TField value);

        /// <summary>
        /// updates an item (one field) by expression member selector filter by provided filterExpression predicate
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="filterExpression"></param>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        /// <remarks>
        /// Even if multiple documents match the filter, only one will be updated because we used UpdateOne
        /// </remarks>
        Task UpdateField<TField>(Expression<Func<TEntity, bool>> filterExpression, 
            Expression<Func<TEntity, TField>> memberExpression, 
            TField value);

        /// <summary>
        /// updates fieldset (filed, value) in the database filter by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldset"></param>
        Task GenericUpdate(object id, Dictionary<string, object> fieldset);

        /// <summary>
        /// update fieldset (filed, value) in the database filter by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="fieldset"></param>
        /// <remarks>address no-sql data provider</remarks>
        Task GenericUpdate(object[] ids, Dictionary<string, object> fieldset);

        /// <summary>
        /// deletes an Entity by id in the current Context
        /// </summary>
        /// <param name="id">the key</param>
        Task DeleteById(params object[] id);

        /// <summary>
        /// deletes an Entity by changing the state of it
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// deletes multiple Entities filter by predicate (in current Context)
        /// </summary>
        /// <param name="predicate">represent expression to filter delete</param>
        void DeleteMany(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// commit changes to underlying database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>if commit changes the state of the database return success Result, otherwise return error Result</returns>
        Task<Result> CommitAsync(CancellationToken cancellationToken);

    }
}
