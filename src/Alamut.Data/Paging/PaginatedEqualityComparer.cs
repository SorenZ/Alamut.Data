using System.Collections.Generic;

namespace Alamut.Data.Paging
{
    public class PaginatedEqualityComparer<T> : IEqualityComparer<IPaginated<T>>
    {
        public bool Equals(IPaginated<T> x, IPaginated<T> y)
        {
            if (x == null) {return false;}
            if (y == null) {return false;}

            return Equals(x.Data.Count, y.Data.Count) &&
                   Equals(x.CurrentPage, y.CurrentPage) &&
                   Equals(x.IsFirstPage, y.IsFirstPage) &&
                   Equals(x.IsLastPage, y.IsLastPage) &&
                   Equals(x.NextPage, y.NextPage) &&
                   Equals(x.PageCount, y.PageCount) &&
                   Equals(x.PageSize, y.PageSize) &&
                   Equals(x.PreviousPage, y.PreviousPage) &&
                   Equals(x.TotalRowsCount, y.TotalRowsCount);
        }

        public int GetHashCode(IPaginated<T> obj)
        {
            throw new System.NotImplementedException();
        }
    }
}