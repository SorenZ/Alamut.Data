using System.Collections.Generic;

namespace Alamut.Data.NoSql
{
    /// <summary>
    /// determine criteria for
    /// - projection
    /// - predicate
    /// - sorts
    /// </summary>
    public class DynamicCriteria
    {
        /// <summary>
        /// projection
        /// used to select only specific fields for output
        /// </summary>
        //public ICollection<string> Fields { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// "City == \"Paris\""
        /// "City == @0", cityToSearch
        /// "City == \"Paris\" && Age > 50"
        /// "City == @0 and Age > @1", "Paris", 50
        /// </example>
        public string FilterClause { get; set; }

        /// <summary>
        /// predicate
        /// used to filter out put
        /// </summary>
        /// <example>
        /// "City == @0", cityToSearch
        /// "City == @0 and Age > @1", "Paris", 50
        /// </example>
        public object[] FilterParameters { get; set; }

        /// <summary>
        /// Sort description 
        /// </summary>
        /// <example>
        /// "City, CompanyName"
        /// "City, CompanyName desc"
        /// </example>
        public string Sorts { get; set; }

        /// <summary>
        /// includes external collection to current entity as a relationship
        /// </summary>
        public IEnumerable<string> Includes { get; set; }
    }
}
