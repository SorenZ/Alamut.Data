using System.Collections.Generic;

namespace Alamut.Data.Paging
{
    public interface IPaginated<T> : IPaginated
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        IList<T> Data { get; }
    }

    public interface IPaginated
    {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value> The current page. </value>
        int CurrentPage { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is first page.
        /// </summary>
        /// <value> <c>true</c> if this instance is first page; otherwise, <c>false</c> . </value>
        bool IsFirstPage { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is last page.
        /// </summary>
        /// <value> <c>true</c> if this instance is last page; otherwise, <c>false</c> . </value>
        bool IsLastPage { get; }

        /// <summary>
        /// Gets the next page.
        /// </summary>
        int NextPage { get; }

        /// <summary>
        /// Gets the page count.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value> The size of the page. </value>
        int PageSize { get; }

        /// <summary>
        /// Gets the previous page.
        /// </summary>
        int PreviousPage { get; }

        /// <summary>
        /// Gets the total rows count.
        /// </summary>
        long TotalRowsCount { get; }
    }
}