using Alamut.Data.Paging;

namespace Alamut.Data.NoSql
{
    public class DynamicPaginatedCriteria : DynamicCriteria, IPaginatedCriteria
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedCriteria" /> class.
        /// </summary>
        public DynamicPaginatedCriteria()
        {
            this.PageSize = 10;
            this.CurrentPage = 1;
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
        public int StartIndex => (this.CurrentPage - 1) * this.PageSize;
    }
}