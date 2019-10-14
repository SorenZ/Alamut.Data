using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.Paging
{
    public static class QueryablePaginatedExtensions
    {
        /// <summary>
        /// Gets the paginated data.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="query"> The query. </param>
        /// <param name="startIndex"> Start index of the row. </param>
        /// <param name="itemCount"> Size of the page. </param>
        /// <returns> </returns>
        public static IQueryable<T> ToPage<T>(this IQueryable<T> query, int startIndex, int itemCount)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (startIndex < 0)
                startIndex = 0;

            return query.Skip(startIndex).Take(itemCount);
        }

        public static IQueryable<T> ToPage<T>(this IQueryable<T> query, IPaginatedCriteria criteria)
        {
            return query.ToPage(criteria.StartIndex, criteria.PageSize);
        }

        /// <summary>
        /// Creates an <see cref="IPaginated{T}" /> instance from the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="paginatedCriteria">The paginated criteria.</param>
        /// <returns></returns>
        public static IPaginated<T> ToPaginated<T>(this IQueryable<T> query, IPaginatedCriteria paginatedCriteria)
        {
            return new Paginated<T>(
                query.ToPage(paginatedCriteria.StartIndex, paginatedCriteria.PageSize).ToList(),
                query.Count(),
                paginatedCriteria.CurrentPage,
                paginatedCriteria.PageSize);
        }

        /// <summary>
        /// Creates an <see cref="IPaginated{T}" /> instance from the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="paginatedCriteria">The paginated criteria.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IPaginated<T>> ToPaginatedAsync<T>(this IQueryable<T> query, 
        IPaginatedCriteria paginatedCriteria, CancellationToken cancellationToken = default)
        {
            return new Paginated<T>(
                await query.ToPage(paginatedCriteria.StartIndex, paginatedCriteria.PageSize).ToListAsync(cancellationToken),
                await query.CountAsync(cancellationToken),
                paginatedCriteria.CurrentPage,
                paginatedCriteria.PageSize);
        }

    }
}
