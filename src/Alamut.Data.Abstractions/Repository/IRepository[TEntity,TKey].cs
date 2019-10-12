using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Alamut.Abstractions.Structure;
using Alamut.Data.Abstractions.Entity;
using Alamut.Data.Abstractions.Paging;

namespace Alamut.Data.Abstractions.Repository
{
    /// <summary>
    /// represents complete repository methods to query and manipulate the database
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TEntity,TKey> where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// provide a queryable source of elements
        /// the Queryable has not been tracked in sql repositories
        /// </summary>
        IQueryable<TEntity> Queryable { get; }

        /// <summary>
        /// get an item by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(TKey id);

        /// <summary>
        /// get an item by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// get an item (selected fields bye projection) by id
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TResult Get<TResult>(TKey id);

        /// <summary>
        /// get an item (selected fields by projection) by predicate
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TResult Get<TResult>(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// get all items 
        /// </summary>
        /// could be true, false, null
        /// null -> not important 
        /// <returns></returns>
        List<TEntity> GetAll();

        /// <summary>
        /// get a list of projected item
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        List<TResult> GetAll<TResult>();


        /// <summary>
        /// get a list of items by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// get a list of items by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<TEntity> GetMany(IEnumerable<TKey> ids);

        /// <summary>
        /// get a list of items (selected fields) by predicate
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<TResult> GetMany<TResult>(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// get filtered item
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<TResult> GetMany<TResult>(IEnumerable<TKey> ids);


        /// <summary>
        /// get items paginated by criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IPaginated<TEntity> GetPaginated(IPaginatedCriteria criteria = null);

        /// <summary>
        /// create an item 
        /// and commit into database.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">save changes into database</param>
        void Create(TEntity entity, bool commit = true);

        /// <summary>
        /// add list of item into database
        /// and commit into database.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="commit"></param>
        void AddRange(IEnumerable<TEntity> list, bool commit = true);

        /// <summary>
        /// update item total value
        /// and commit into database.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        void Update(TEntity entity, bool commit = true);

        /// <summary>
        /// update an item (one field) by expression member selector by id
        /// and commit into database.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="id"></param>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        /// <remarks>
        /// Even if multiple documents match the filter, only one will be updated because we used UpdateOne
        /// </remarks>
        void UpdateOne<TField>(TKey id, 
            Expression<Func<TEntity, TField>> memberExpression, 
            TField value);

        /// <summary>
        /// update an item (one field) by expression member selector (select item by predicate)
        /// and commit into database.
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
            Expression<Func<TEntity, TField>> memberExpression, TField value);

        /// <summary>
        /// update fieldset in the database by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldset"></param>
        /// <remarks>address no-sql data provider</remarks>
        void GenericUpdate(TKey id, Dictionary<string, dynamic> fieldset);

        /// <summary>
        /// add an item to a list (if not exist)
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="id"></param>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        /// <remarks>address no-sql data provider</remarks>
        void AddToList<TValue>(TKey id, 
            Expression<Func<TEntity, IEnumerable<TValue>>> memberExpression, 
            TValue value);

        /// <summary>
        /// remove an item from a list (all item if same)
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="id"></param>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        /// <remarks>address no-sql data provider</remarks>
        void RemoveFromList<TValue>(TKey id, 
            Expression<Func<TEntity, IEnumerable<TValue>>> memberExpression, 
            TValue value);

        /// <summary>
        /// Deletes an item by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commit"></param> 
        void Delete(TKey id, bool commit = true);

        /// <summary>
        /// Deletes multiple documents.
        /// </summary>
        /// <param name="predicate">represent expression to filter delete</param>
        /// <param name="commit"></param>
        void DeleteMany(Expression<Func<TEntity, bool>> predicate
            , bool commit = true);
    }
}
