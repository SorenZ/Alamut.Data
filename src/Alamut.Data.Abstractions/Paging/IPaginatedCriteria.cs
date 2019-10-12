namespace Alamut.Data.Abstractions.Paging
{
    /// <summary>
    /// Represents the criteria used to show a paginated data.
    /// </summary>
    public interface IPaginatedCriteria
    {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value> The current page. </value>
        int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        /// The default value is 10.
        /// <remarks>
        /// </remarks>
        int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the sort descriptions.
        /// </summary>
        //SortDescription[] SortDescriptions { get; set; }

        /// <summary>
        /// Gets the start index according to the current page and page size.
        /// </summary>
        int StartIndex { get; }
        
    }
}
