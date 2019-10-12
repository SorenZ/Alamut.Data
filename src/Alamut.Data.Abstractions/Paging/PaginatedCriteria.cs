namespace Alamut.Data.Abstractions.Paging
{
    /// <summary>
    /// Represents the criteria used to show a paginated data.
    /// </summary>
    /// <remarks>
    /// The <see cref="PaginatedCriteria"/> properties are intentionally left without property change notification.
    /// </remarks>
    public class PaginatedCriteria : IPaginatedCriteria
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedCriteria" /> class.
        /// </summary>
        public PaginatedCriteria(int currentPage = 1, int pageSize = 10)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <remarks>
        /// Starts from 1.
        /// </remarks>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        /// <remarks>
        /// The default value is 10.
        /// </remarks>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the start index according to the current page and page size.
        /// </summary>
        public int StartIndex => (this.CurrentPage - 1)*this.PageSize;
    }
}
