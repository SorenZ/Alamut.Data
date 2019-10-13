using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Alamut.Data.Abstractions.Entity;

namespace Alamut.Data.Abstractions.Repository
{
    public interface INoSqlRepository<TEntity, in TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
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
    }
}
