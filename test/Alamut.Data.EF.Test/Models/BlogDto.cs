using System.Collections.Generic;

namespace Alamut.Data.EF.Test.Models
{
    public class BlogDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
    }

    public class BlogDtoIEqualityComparer : IEqualityComparer<BlogDto>
    {
        /// <inheritdoc />
        public bool Equals(BlogDto x, BlogDto y)
        {
            return y != null && (x != null && (x.Id.Equals(y.Id) &&
                                               x.Rating.Equals(y.Rating)));
        }

        /// <inheritdoc />
        public int GetHashCode(BlogDto obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
