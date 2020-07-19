using System.Linq;
using System.Linq.Dynamic.Core;

namespace Alamut.Data.NoSql
{
    public static class DynamicCriteriaExtensions
    {
        public static IQueryable<T> ApplyDynamicCriteria<T>(this IQueryable<T> query, DynamicCriteria criteria)
        {
            return query
                .ApplyFilter(criteria.FilterClause, criteria.FilterParameters)
                .ApplySort(criteria.Sorts);
        }

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string sorts) =>
            string.IsNullOrEmpty(sorts) ? query : query.OrderBy(sorts);

        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, string filter, object[] parameters) =>
            string.IsNullOrEmpty(filter) ? query : query.Where(filter, parameters);
    }
}
