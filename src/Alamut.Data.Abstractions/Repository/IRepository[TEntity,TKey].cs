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
    /// represents complete repository methods to query and manipulate the database
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TEntity, in TKey> where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// provide a queryable source of elements
        /// the Queryable has not been tracked in sql repositories
        /// </summary>
        IQueryable<TEntity> Queryable { get; }

        /// <summary>
        /// gets an Entity by id 
        /// </summary>
        /// <param name="id">entity key</param>
        /// <returns></returns>
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
        /// get all Entities filtered by predicate
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
        /// adds an Entity to the context
        /// and commit into database (if commit set to true).
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">save changes into database</param>
        void Create(TEntity entity, bool commit = true);
        Task CreateAsync(TEntity entity, bool commit = true);


        /// <summary>
        /// adds a list of Entities to the context
        /// and commit into database (if commit set to true).
        /// </summary>
        /// <param name="list"></param>
        /// <param name="commit"></param>
        void AddRange(IEnumerable<TEntity> list, bool commit = true);
        Task AddRangeAsync(IEnumerable<TEntity> list, bool commit = true);

        /// <summary>
        /// updates an item to the context
        /// and commit into database (if commit set to true).
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        void Update(TEntity entity, bool commit = true);
        Task UpdateAsync(TEntity entity, bool commit = true);

        /// <summary>
        /// updates an item (one field) by expression member selector filter by id
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="id"></param>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        /// <param name="commit">commit into database (if it sets to true)</param>
        /// <remarks>
        /// Even if multiple documents match the filter, only one will be updated because we used UpdateOne
        /// </remarks>
        void UpdateOne<TField>(TKey id, 
            Expression<Func<TEntity, TField>> memberExpression, 
            TField value, 
            bool commit = true);
        Task UpdateOneAsync<TField>(TKey id, 
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
        /// <param name="commit">commit into database (if it sets to true)</param>
        /// <remarks>
        /// Even if multiple documents match the filter, only one will be updated because we used UpdateOne
        /// </remarks>
        void UpdateOne<TFilter, TField>(Expression<Func<TEntity, bool>> filterExpression, 
            Expression<Func<TEntity, TField>> memberExpression, 
            TField value,
            bool commit = true);
        Task UpdateOneAsync<TFilter, TField>(Expression<Func<TEntity, bool>> filterExpression, 
            Expression<Func<TEntity, TField>> memberExpression, 
            TField value,
            bool commit = true);

        /// <summary>
        /// update fieldset (filed, value) in the database filter by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldset"></param>
        /// <param name="commit">commit into database (if it sets to true)</param>
        /// <remarks>address no-sql data provider</remarks>
        void GenericUpdate(TKey id, Dictionary<string, dynamic> fieldset, bool commit = true);
        Task GenericUpdateAsync(TKey id, Dictionary<string, dynamic> fieldset, bool commit = true);

        /// <summary>
        /// deletes an Entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commit"></param> 
        void Delete(TKey id, bool commit = true);
        Task DeleteAsync(TKey id, bool commit = true);

        /// <summary>
        /// deletes multiple documents filter by predicate.
        /// </summary>
        /// <param name="predicate">represent expression to filter delete</param>
        /// <param name="commit">commit into database (if it sets to true)</param>
        void DeleteMany(Expression<Func<TEntity, bool>> predicate, bool commit = true);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate, bool commit = true);
    }
}
