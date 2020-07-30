using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alamut.Data.Paging;
using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.NoSql
{
    public static class DynamicCriteriaPaginationExtensions
    {
        /// <summary>
        /// Creates an <see cref="IPaginated{T}" /> instance from the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static IPaginated<T> ToPaginated<T>(this IQueryable<T> query, DynamicPaginatedCriteria criteria) =>
            query.ApplyCriteria(criteria)
                .ToPaginated(criteria as IPaginatedCriteria);

        /// <summary>
        /// Creates an <see cref="IPaginated{T}" /> instance from the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="criteria"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IPaginated<T>> ToPaginatedAsync<T>(this IQueryable<T> query, 
            DynamicPaginatedCriteria criteria, CancellationToken cancellationToken) =>
            await query.ApplyCriteria(criteria)
                .ToPaginatedAsync(criteria as IPaginatedCriteria, cancellationToken);
    }
}