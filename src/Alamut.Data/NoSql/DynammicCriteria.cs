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
        public ICollection<string> Fields { get; set; }

        /// <summary>
        /// predicate
        /// used to filter out put
        /// </summary>
        public Dictionary<string, dynamic> Filters { get; set; }

        /// <summary>
        /// order
        /// set order in dictionary like collection
        /// key : fields name
        /// value : 1 -> ascending; -1 -> descending
        /// </summary>
        public Dictionary<string, int> Sorts { get; set; }

        /// <summary>
        /// includes external collection to current entity as a relationship
        /// dic :
        /// - key -> key of external collection
        /// - value -> the property should fill with the key
        /// </summary>
        public Dictionary<string, string> Includes { get; set; }
    }
}
