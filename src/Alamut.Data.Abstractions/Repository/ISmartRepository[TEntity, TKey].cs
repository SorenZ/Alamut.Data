using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Alamut.Data.Abstractions.Entity;

namespace Alamut.Data.Abstractions.Repository
{
    /// <summary>
    /// provides repository helpers based on AutoMapper mapping configuration 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface ISmartRepository<TEntity, in TKey> : IRepository<TEntity, TKey> 
        where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// gets an item (mapped to provided TResult) by id
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TResult GetById<TResult>(TKey id);
        Task<TResult> GetByIdAsync<TResult>(TKey id);

        /// <summary>
        /// gets an item (mapped to provided TResult) filter by provided predicate 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TResult Get<TResult>(Expression<Func<TEntity, bool>> predicate);
        Task<TResult> GetAsync<TResult>(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// gets all items (mapped to provided TResult)  
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        List<TResult> GetAll<TResult>();
        Task<List<TResult>> GetAllAsync<TResult>();

        /// <summary>
        /// gets a list of items (mapped to provided TResult) filter by provided predicate
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<TResult> GetMany<TResult>(Expression<Func<TEntity, bool>> predicate);
        Task<List<TResult>> GetManyAsync<TResult>(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// gets a list of items (mapped to provided TResult) filter by provided ids
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<TResult> GetMany<TResult>(IEnumerable<TKey> ids);
        Task<List<TResult>> GetManyAsync<TResult>(IEnumerable<TKey> ids);
    }
}
