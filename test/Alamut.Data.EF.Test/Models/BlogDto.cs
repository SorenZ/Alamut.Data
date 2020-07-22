using System.Collections.Generic;

namespace Alamut.Data.EF.Test.Models
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        public int Rating { get; set; }
    }

    public class BlogDtoIEqualityComparer : IEqualityComparer<BlogViewModel>
    {
        /// <inheritdoc />
        public bool Equals(BlogViewModel x, BlogViewModel y)
        {
            return y != null && (x != null && (x.Id.Equals(y.Id) &&
                                               x.Rating.Equals(y.Rating)));
        }

        /// <inheritdoc />
        public int GetHashCode(BlogViewModel obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
