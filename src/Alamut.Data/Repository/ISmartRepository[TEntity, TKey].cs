using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Alamut.Data.Entity;
using Alamut.Data.Paging;

namespace Alamut.Data.Repository
{
    /// <summary>
    /// provides repository helpers based on AutoMapper mapping configuration 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface ISmartRepository<TEntity, in TKey> : ISmartRepository<TEntity> 
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// gets an item (mapped to provided TDto) by id
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TDto> GetById<TDto>(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// gets an item (mapped to provided TDto) filter by provided predicate 
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TDto> Get<TDto>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// gets all items (mapped to provided TDto)  
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <returns></returns>
        Task<List<TDto>> GetAll<TDto>(CancellationToken cancellationToken = default);

        /// <summary>
        /// gets a list of items (mapped to provided TDto) filter by provided predicate
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TDto>> GetMany<TDto>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// gets a list of items (mapped to provided TDto) filter by provided ids
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TDto>> GetByIds<TDto>(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// gets a list of requested DTO in Paginated data-type filtered by provided criteria or default 
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPaginated<TDto>> GetPaginated<TDto>(IPaginatedCriteria criteria = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// maps the provided DTO to the Entity and add it to the current context 
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="dto"></param>
        /// <returns>generated Entity based on provided DTO</returns>
        TEntity Add<TDto>(TDto dto);


        /// <summary>
        /// maps the provided DTO to the Entity and update it to the current context 
        /// </summary>
        /// <param name="id">the key</param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>generated Entity based on provided DTO</returns>
        Task<TEntity> UpdateById<TDto>(TKey id,TDto dto, CancellationToken cancellationToken = default);

        // ReSharper disable once InvalidXmlDocComment
        /// <summary>
        /// try to find the Entity by provided id
        /// if exist -> update it by provided DTO 
        /// if not exist -> maps the provided DTO to Entity 
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        //Task AddOrUpdate<TDto>(TKey id, TDto dto);
    }
}
