using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Alamut.Data.Abstractions.Entity;
using Alamut.Data.Abstractions.Paging;

namespace Alamut.Data.Abstractions.Repository
{
    /// <summary>
    /// represents complete repository methods to query and manipulate the context or underlying database
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TEntity, in TKey> where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// provides a queryable source of elements
        /// the Queryable has not been tracked in sql repositories
        /// </summary>
        IQueryable<TEntity> Queryable { get; }

        /// <summary>
        /// gets an Entity by id 
        /// </summary>
        /// <param name="id">entity key</param>
        /// <returns>an Entity or null</returns>
        TEntity GetById(TKey id);
        Task<TEntity> GetByIdAsync(TKey id);

        /// <summary>
        /// gets an Entity filtered by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>an entity or null</returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// gets all Entities  
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// gets all Entities filtered by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// gets all entities filtered by provided ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<TEntity> GetMany(IEnumerable<TKey> ids);
        Task<List<TEntity>> GetManyAsync(IEnumerable<TKey> ids);

        /// <summary>
        /// gets a list of Entities in Paginated data-type filtered by provided criteria or default 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IPaginated<TEntity> GetPaginated(IPaginatedCriteria criteria = null);
        Task<IPaginated<TEntity>> GetPaginatedAsync(IPaginatedCriteria criteria = null);

        /// <summary>
        /// adds an Entity to the current context
        /// </summary>
        /// <param name="entity"></param>
        void Create(TEntity entity);
        
        /// <summary>
        /// adds an Entity to the underlying database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task CreateAndCommit(TEntity entity);


        /// <summary>
        /// adds a list of Entities to the current Context
        /// </summary>
        /// <param name="list"></param>
        void AddRange(IEnumerable<TEntity> list);
        
        /// <summary>
        /// adds a list of Entities to the underlying Database
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task AddRangeAndCommit(IEnumerable<TEntity> list);

        /// <summary>
        /// updates an Entity to the current Context
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// update an Entity to the underlying Database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAndCommit(TEntity entity);

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
        void UpdateOne<TField>(TKey id,
            Expression<Func<TEntity, TField>> memberExpression,
            TField value);

        /// <summary>
        /// updates an item (one field) by expression member selector filter by id
        /// and commit changes to underlying Database
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="id">the key</param>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        Task UpdateOneAndCommit<TField>(TKey id,
            Expression<Func<TEntity, TField>> memberExpression,
            TField value,
            bool commit = true);

        /// <summary>
        /// update an item (one field) by expression member selector filter by provided filterExpression predicate
        /// </summary>
        /// <typeparam name="TFilter"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="filterExpression"></param>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        /// <remarks>
        /// Even if multiple documents match the filter, only one will be updated because we used UpdateOne
        /// </remarks>
        void UpdateOne<TFilter, TField>(Expression<Func<TEntity, bool>> filterExpression, 
            Expression<Func<TEntity, TField>> memberExpression, 
            TField value);

        /// <summary>
        /// update an item (one field) by expression member selector filter by provided filterExpression predicate
        /// and commit changes to underlying Database
        /// </summary>
        /// <typeparam name="TFilter"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="filterExpression"></param>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UpdateOneAndCommit<TFilter, TField>(Expression<Func<TEntity, bool>> filterExpression, 
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
        /// update fieldset (filed, value) in the database filter by id
        /// and commit changes to underlying Database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldset"></param>
        /// <returns></returns>
        Task GenericUpdateAndCommit(TKey id, Dictionary<string, object> fieldset);

        /// <summary>
        /// deletes an Entity by id in the current Context
        /// </summary>
        /// <param name="id">the key</param>
        void DeleteById(TKey id);
        
        /// <summary>
        /// deletes an Entity by id in the underlying Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteByIdAndCommit(TKey id);

        /// <summary>
        /// deletes multiple Entities filter by predicate (in current Context)
        /// </summary>
        /// <param name="predicate">represent expression to filter delete</param>
        void DeleteMany(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// deletes multiple Entities filter by predicate in underlying Database
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task DeleteManyAndCommit(Expression<Func<TEntity, bool>> predicate);
    }
}
