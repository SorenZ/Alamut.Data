using System.Linq;
using System.Linq.Dynamic.Core;

namespace Alamut.Data.NoSql
{
    public static class DynamicCriteriaExtensions
    {
        public static IQueryable<T> ApplyCriteria<T>(this IQueryable<T> query, DynamicCriteria criteria)
        {
            return query
                .Filter(criteria.FilterClause, criteria.FilterParameters)
                .Sort(criteria.Sorts);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string sorts) =>
            string.IsNullOrEmpty(sorts) ? query : query.OrderBy(sorts);

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, string filter, object[] parameters) =>
            string.IsNullOrEmpty(filter) ? query : query.Where(filter, parameters);
    }
}
