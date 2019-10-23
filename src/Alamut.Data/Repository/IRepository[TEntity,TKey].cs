using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Alamut.Abstractions.Structure;
using Alamut.Data.Entity;
using Alamut.Data.Paging;

namespace Alamut.Data.Repository
{
    /// <summary>
    /// represents complete repository methods to query and manipulate the context or underlying database
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TEntity, in TKey> : IRepository<TEntity>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// gets an Entity by id 
        /// </summary>
        /// <param name="id">entity key</param>
        /// <param name="cancellationToken"></param>
        /// <returns>an Entity or null</returns>
        Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// gets all entities filtered by provided ids
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetByIds(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

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
        Task UpdateFieldById<TField>(TKey id,
            Expression<Func<TEntity, TField>> memberExpression,
            TField value);

        /// <summary>
        /// update fieldset (filed, value) in the database filter by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldset"></param>
        /// <remarks>address no-sql data provider</remarks>
        void GenericUpdate(TKey id, Dictionary<string, object> fieldset);

        /// <summary>
        /// deletes an Entity by id in the current Context
        /// </summary>
        /// <param name="id">the key</param>
        void DeleteById(TKey id);
    }
}
