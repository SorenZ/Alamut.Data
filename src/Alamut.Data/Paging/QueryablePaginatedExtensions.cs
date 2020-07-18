using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alamut.Data.NoSql;
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
            var data = await query.ToPage(paginatedCriteria)
                .ToListAsync(cancellationToken);

            var count = await query.CountAsync(cancellationToken);

            // await Task.WhenAll(dataTask, countTask);
            // "A second operation started on this context before a previous operation completed. This is usually caused by different threads using the same instance of DbContext. For more information on how to avoid threading issues with DbContext, see https://go.microsoft.com/fwlink/?linkid=2097913.

            return new Paginated<T>(
                data,
                count,
                paginatedCriteria.CurrentPage,
                paginatedCriteria.PageSize);
        }


        /// <summary>
        /// Creates an <see cref="IPaginated{T}" /> instance from the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="criteria"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IPaginated<T>> ApplyDynamicPaginatedAsync<T>(this IQueryable<T> query, 
            DynamicPaginatedCriteria criteria, CancellationToken cancellationToken)
        {
            return await query.ApplyDynamicCriteria(criteria)
                .ToPaginatedAsync(criteria, cancellationToken);
        }

    }
}
