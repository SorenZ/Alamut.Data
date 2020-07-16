using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Alamut.Data.Entity;

namespace Alamut.Data.Repository
{
    /// <summary>
    /// provides repository helpers based on AutoMapper mapping configuration 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface ISmartRepository<TEntity, in TKey> : IRepository<TEntity, TKey>,
        ISmartRepository<TEntity> 
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
        /// gets a list of items (mapped to provided TDto) filter by provided ids
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TDto>> GetByIds<TDto>(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// maps the provided DTO to the Entity and update it to the current context 
        /// </summary>
        /// <param name="id">the key</param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>generated Entity based on provided DTO</returns>
        Task<TEntity> UpdateById<TDto>(TKey id,TDto dto, CancellationToken cancellationToken = default);

    }
}
