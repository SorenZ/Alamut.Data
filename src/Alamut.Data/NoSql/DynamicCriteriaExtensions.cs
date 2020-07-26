using System.Linq;
using System.Linq.Dynamic.Core;

namespace Alamut.Data.NoSql
{
    public static class DynamicCriteriaExtensions
    {
        /// <summary>
        /// apply dynamic criteria on underlying data source
        /// </summary>
        /// <param name="query"></param>
        /// <param name="criteria"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> ApplyCriteria<T>(this IQueryable<T> query, DynamicCriteria criteria)
        {
            return query
                .Filter(criteria.FilterClause, criteria.FilterParameters)
                .Sort(criteria.Sorts);
        }

        
        /// <summary>
        /// Sorts underlying data source by provided sorts description
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sorts"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>sorted queryable (IOrderedQueryable)</returns>
        /// <example>
        /// "City, CompanyName"
        /// "City, CompanyName desc"
        /// </example> 
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string sorts) =>
            string.IsNullOrEmpty(sorts) ? query : query.OrderBy(sorts);

        
        /// <summary>
        /// Filters underlying data source by provided filter clause and parameters 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <param name="parameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <example>
        /// "City == @0", cityToSearch
        /// "City == @0 and Age > @1", "Paris", 50
        /// </example> 
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, string filter, params object[] parameters) =>
            string.IsNullOrEmpty(filter) ? query : query.Where(filter, parameters);
    }
}
