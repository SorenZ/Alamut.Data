using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Alamut.Data.NoSql;
using Alamut.Data.Paging;

namespace Alamut.Data.Repository
{
    public interface ISmartRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
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
        /// gets a list of requested DTO in Paginated data-type filtered by provided criteria or default 
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPaginated<TDto>> GetPaginated<TDto>(CancellationToken cancellationToken);
        
        Task<IPaginated<TDto>> GetPaginated<TDto>(IPaginatedCriteria criteria, CancellationToken cancellationToken);
        
        Task<IPaginated<TDto>> GetPaginated<TDto>(DynamicPaginatedCriteria criteria, CancellationToken cancellationToken);

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
        Task<TEntity> UpdateById<TDto>(object id,TDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// maps the provided DTO to the Entity and update it to the current context 
        /// </summary>
        /// <param name="ids">the keys</param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>generated Entity based on provided DTO</returns>
        Task<TEntity> UpdateById<TDto>(object[] ids,TDto dto, CancellationToken cancellationToken = default);

    }
}