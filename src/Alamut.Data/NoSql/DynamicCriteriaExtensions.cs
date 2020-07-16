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



    //public static class CriteriaSorts
    //{
    //    public static IOrderedQueryable<T> OrderBy<T>(
    //        this IQueryable<T> source, 
    //        string property)
    //    {
    //        return ApplyOrder<T>(source, property, "OrderBy");
    //    }

    //    public static IOrderedQueryable<T> OrderByDescending<T>(
    //        this IQueryable<T> source, 
    //        string property)
    //    {
    //        return ApplyOrder<T>(source, property, "OrderByDescending");
    //    }

    //    public static IOrderedQueryable<T> ThenBy<T>(
    //        this IOrderedQueryable<T> source, 
    //        string property)
    //    {
    //        return ApplyOrder<T>(source, property, "ThenBy");
    //    }

    //    public static IOrderedQueryable<T> ThenByDescending<T>(
    //        this IOrderedQueryable<T> source, 
    //        string property)
    //    {
    //        return ApplyOrder<T>(source, property, "ThenByDescending");
    //    }

    //    static IOrderedQueryable<T> ApplyOrder<T>(
    //        IQueryable<T> source, 
    //        string property, 
    //        string methodName) 
    //    {
    //        string[] props = property.Split('.');
    //        Type type = typeof(T);
    //        ParameterExpression arg = Expression.Parameter(type, "x");
    //        Expression expr = arg;
    //        foreach(string prop in props) {
    //            // use reflection (not ComponentModel) to mirror LINQ
    //            PropertyInfo pi = type.GetProperty(prop);
    //            expr = Expression.Property(expr, pi);
    //            type = pi.PropertyType;
    //        }
    //        Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
    //        LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

    //        object result = typeof(Queryable).GetMethods().Single(
    //                method => method.Name == methodName
    //                          && method.IsGenericMethodDefinition
    //                          && method.GetGenericArguments().Length == 2
    //                          && method.GetParameters().Length == 2)
    //            .MakeGenericMethod(typeof(T), type)
    //            .Invoke(null, new object[] {source, lambda});
    //        return (IOrderedQueryable<T>)result;
    //    }
    //}

}
